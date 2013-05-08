<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<title>Untitled Document</title>
</head>
<body onMouseMove="startMoving(event);" onMouseUp="stopMoving();">
<script language="javascript">
    var dragMode = false;
    var obj = ""
    function toDragMode(what) {
        dragMode = true;
        obj = what;
    }
    function startMoving(e) {
        if (!dragMode) return false;
        obj.style.width = (e.clientX - 20) + 'px';
    }
    function stopMoving() {
        dragMode = false;
        obj = "";
    }
</script>
<h1>Click-and-drag on header cells to resize column widths</h1>
<table id="table2" border="1">
<tr>
<th onMouseOver="this.style.cursor='e-resize';" onMouseDown="toDragMode(this);" onMouseUp="stopMoving();">click-and-drag to resize</th>
<th onMouseOver="this.style.cursor='e-resize';" onMouseDown="toDragMode(this);" onMouseUp="stopMoving();">click-and-drag to resize</th>
</tr>
<tr>
<td>Lorem</td>
<td>Ipsum</td>
</tr>
<tr>
<td>Dolor</td>
<td>Sit amet</td>
</tr>van chu
</table>
<p>
<a href="#" onclick="history.go()">Resume sizes</a>
</p>
</body>
</html>