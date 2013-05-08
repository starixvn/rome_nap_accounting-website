// Set URL of your WebSocketMain.swf here, for web-socket-js
WEB_SOCKET_SWF_LOCATION = 'Script/websocket/WebSocketMainInsecure.swf';
var SK, Notify = {}, tIntervalConn = null, nCountConn = 0, NotifyCallback = function(data) { };
///
// ==== Connect Notify Server ===
///
// options = {SK: 'string', MaxQ: 'number', Interval: 'number'}
function ConnectNotify(opts, callback) {
    // collect information
    SK = typeof (opts.SK) == undefined ? '' : opts.SK;
    if (SK.length==0) { alert('Secret key is required by notify service.');  return; }
    MaxQueue = typeof (opts.MaxQ) == undefined ? MaxQueue : opts.MaxQ;
    TInterval = typeof (opts.Interval) == undefined ? TInterval : opts.Interval;
    NotifyCallback = typeof (callback) == undefined ? NotifyCallback : callback;
    
    // If we're using the Flash fallback, we need Flash.
    if (!window.WebSocket && getFlashVersion().split(',').shift() < 10) {
        alert('Flash Player >= 10 is required.');
        return;
    }

    // Set up the NotifyServer client object
    Notify = new NotifyServer({
        Server: '10.18.200.201',
        //Server: '10.19.10.61',
        Action: 'connect-'.concat(SK),
        Port: 8088,
        DebugMode: true,
        ConnectMode: { Auto: true, Interval: 60000 }
    });

    Notify.Connected = function() {
        LogMessage('Connection established!');
        qBet = new QueueBet(MaxQueue, TInterval); qBet.process();
    };

    Notify.Disconnected = function() {
        LogMessage('Connection closed.');
        if (Notify.ConnectMode.Auto && nCountConn < 3) {
            LogMessage('Auto start notify service in ' + Notify.ConnectMode.Interval.toString() + ' miliseconds.');
            window.setTimeout(function() {
                AutoStart();
            }, Notify.ConnectMode.Interval);
        }

    };

    Notify.MessageReceived = function(event) {
        //LogMessage('Message received.');
        ParseData(event.data);
    };

    // Start connection
    LogMessage('Connecting...');
    Notify.Start();
}

function LogMessage(message) {
    if (Notify.DebugMode) {
        var p = $('<p></p>').html(message);
        $('body').append(p);
    }
}

function ParseData(data) {
    if (data) {
        var resp = JSON.parse(data);
        if (resp.Type == 0) { // connect
            var message = resp.Data.Message;
            //LogMessage(message);
        } else if (resp.Type == 1) { // disconnect
            var message = resp.Data.Message;
            //LogMessage(message);
        } else if (resp.Type == 3) { // get data
            //LogMessage(resp.Data);
            if (qBet != null) {
                qBet.receive(resp.Data);
            }
        }
    }
}

function AutoStart() {
    if (Notify.SocketState != Notify.SocketStates.Open) {
        Notify.Start();
        nCountConn++;
    } 
}
//********
/// Queue handler
//********
var tInterval = null, qBet = null, MaxQueue = 500, TInterval=100;
var QueueBet = function(maxLength, interval) {
    this.QueueItem = [];
    this.MaxLength = typeof (maxLength) == undefined ? MaxQueue : maxLength;
    this.Interval = typeof (interval) == undefined ? TInterval : interval;
    window.clearInterval(tInterval);
}
QueueBet.prototype = {
    receive: function(data) {
        if (typeof data != undefined) {
            this.QueueItem.push(data);
            if (this.QueueItem.length >= this.MaxLength) {
                this.QueueItem.shift();
            }
        }
    },
    process: function() {
        LogMessage("Max Q: " + this.MaxLength + " - Time interval: " + this.Interval);
        //Notify.Send(new Filter(3, '>|50'));
        tInterval = window.setInterval(function() {
            if (qBet.QueueItem.length > 0) {
                var data = qBet.QueueItem.pop();
                if (NotifyCallback && data) {
                    try { NotifyCallback(JSON.parse(data)); } catch (e) { alert('Queue: ' + e); }
                }
            }
        }, qBet.Interval);
    }
};
//********
/// Client filtering
/// Type: 1-Name, 2-IP, 3-Stake
//********
var Filter = function(type, data) {
    this.Type = typeof type == undefined ? 1 : type;
    this.Data = typeof data == undefined ? '' : data;
};
