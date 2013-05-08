/***
Notify Websockets Client Library
****/
(function() {
    function NotifyServer(options) {
        if (!this instanceof NotifyServer) {
            return new NotifyServer(options);
        } else {
            if (!options) {
                options = {};
            }

            this._options = MergeDefaults(this._defaultOptions, options);

            if (!options.SocketType) {
                /* Try to autodetect websocket support if we have Modernizr
                loaded. If another lib (like web-sockets-js) is loaded that
                creates a websocket obj where we wouldn't normally have one,
                we'll assume that it's flash.*/
                if (!!Modernizr) {
                    if (Modernizr.websockets) {
                        this.SocketType = NotifyServer.prototype.SocketTypes.WebSocket;
                    } else if (!Modernizr.websockets && !!window.WebSocket) {
                        this.SocketType = NotifyServer.prototype.SocketTypes.FlashSocket;
                    }
                }
            }

            if (!window.WebSocket) {
                throw 'UNSUPPORTED: Websockets are not supported in this browser!';
            }

            this.SocketState = NotifyServer.prototype.SocketStates.Closed;

            this.Connected = this._options.Connected;
            this.Disconnected = this._options.Disconnected;
            this.MessageReceived = this._options.MessageReceived;

            // adding opts
            this.DebugMode = this._options.DebugMode;
            this.ConnectMode = this._options.ConnectMode;
        }
    }

    NotifyServer.prototype = {
        _socket: {},
        _heartbeat: {},
        _lastReceive: (new Date()).getTime(),
        _options: {},

        SocketStates: {
            Connecting: 0,
            Open: 1,
            Closing: 2,
            Closed: 3
        },

        SocketState: 3,

        SocketTypes: {
            WebSocket: 'websocket',
            FlashSocket: 'flashsocket'
        },
        
        DebugMode: false,
        ConnectMode: null,

        Start: function() {
            var server = 'ws://' + this._options.Server + ':' + this._options.Port + '/' + this._options.Action + '/' + this._options.SocketType;
            var ACInstance = this;
            this._socket = new WebSocket(server);
            this._socket.onopen = function() { ACInstance._OnOpen(); };
            this._socket.onmessage = function(data) { ACInstance._OnMessage(data); };
            this._socket.onclose = function() { ACInstance._OnClose(); };
            this.SocketState = NotifyServer.prototype.SocketStates.Connecting;

            if (this._options.DebugMode) {
                console.log('Server started, connecting to ' + server);
            }
        },

        Send: function(data) {
            if (typeof data === 'object') {
                data = JSON.stringify(data);
            }

            this._socket.send(data);

            if (this._options.DebugMode) {
                console.log('Sent data to server: ' + data);
            }
        },

        Stop: function() {
            this._socket.close();

            if (this._options.DebugMode) {
                console.log('Closed websocket connection.');
            }
        },

        Connected: function() { },
        Disconnected: function() { },
        MessageReceived: function() { },
        _OnOpen: function() {
            var instance = this;
            window.clearTimeout(instance._heartbeat);

            this._heartbeat = window.setTimeout(function() { instance._HeartBeat(); }, instance._options.Heartbeat * 1000);

            this.SocketState = NotifyServer.prototype.SocketStates.Open;

            if (this._options.DebugMode) {
                console.log('Connected, hearbeat started (every ' + this._options.Heartbeat + ' seconds).');
            }
            // fire callback
            this.Connected();
        },

        _OnMessage: function(event) {
            var instance = this;
            if (event.data === this._options.HeartbeatPackage) {
                window.clearTimeout(instance._heartbeat);
                this._heartbeat = window.setTimeout(function() { instance._HeartBeat(); }, instance._options.Heartbeat * 1000);
                return;
            }

            this._lastReceive = (new Date()).getTime();

            if (this._options.DebugMode) {
                console.log('Message received: ' + JSON.stringify(event.data));
            }
            // fire callback
            this.MessageReceived(event);
        },

        _OnClose: function() {
            var instance = this;
            if (this._options.DebugMode) {
                console.log('Connection closed. Heartbeat stopped.');
            }

            window.clearTimeout(instance._heartbeat);
            this.SocketState = NotifyServer.prototype.SocketStates.Closed;

            // fire callback
            this.Disconnected();
        },

        _HeartBeat: function() {
            var instance = this;
            if (this._socket.readyState !== undefined && this._socket.readyState === 1) {
                var now = (new Date()).getTime();
                var diff = now - this._lastReceive;

                if (this._options.DebugMode) {
                    console.log('Heartbeat fired. ' + diff + 'ms since the last one.');
                }

                if (diff >= this._options.Heartbeat * 1000) {
                    this._socket.send(instance._options.HeartbeatPackage);
                }
            }

            window.clearTimeout(instance._heartbeat);
            this._heartbeat = window.setTimeout(function() { instance._HeartBeat(); }, instance._options.Heartbeat * 1000);
        }
    };

    NotifyServer.prototype._defaultOptions = {
        Port: 81,
        Heartbeat: 25,
        HeartbeatPackage: '7', //Default for NotifyServer
        Server: '',
        SK: '',
        Action: '',
        SocketType: NotifyServer.prototype.SocketTypes.WebSocket,

        Connected: function() { },
        Disconnected: function() { },
        MessageReceived: function(data) { },
        ConnectMode: { Auto: false, Interval: 60000 },

        DebugMode: false
    };

    function MergeDefaults(o1, o2) {
        var o3 = {};
        var p = {};

        for (p in o1) {
            o3[p] = o1[p];
        }

        for (p in o2) {
            o3[p] = o2[p];
        }

        return o3;
    }

    window.NotifyServer = NotifyServer;
    window.MergeDefaults = MergeDefaults;
})(window);
