<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html>
<head>
	<title> jquery.grid.columnSizing</title>
	<meta name="Generator" content="EditPlus">
	<meta name="Author" content="Enrique Melendez Estrada">
	<meta name="Keywords" content="jquery javascript grid columns collapsible collapse plugin">
	<meta name="Description" content="">
	

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
	<script type="text/javascript" src="../js/jQuery/Table/Resize/jquery.dimensions.pack.js"></script>
	<script type="text/javascript" src="../js/jQuery/Table/Resize/jquery.cookies.pack.js"></script>
	<script type="text/javascript" src="../js/jQuery/Table/Resize/jquery.iutil.pack.js"></script>
	<script type="text/javascript" src="../js/jQuery/Table/Resize/jquery.idrag.js"></script>
	<script type="text/javascript" src="../js/jQuery/Table/Resize/jquery.grid.columnSizing.js"></script>
	<script type="text/javascript">

	    $(document).ready(function () {
	        $("table.tbHeader").columnSizing({
	            viewResize: false,
	            viewGhost: false,
	            tableWidthFixed: true
	            //tbBody: $("table.content")
	        }).end()
	    }
        );
	</script>
	

	
	<style>
		html,
		body,
		table{
			font : normal 8pt verdana;	
			padding : 0;
			margin : 0;
			border : 0 none transparent;
		}
		/** alucinante, consigue LAYOUT para IE... **/
		#ITAtab,
		#ITAtab ul{
		 zoom:1;
		}
		#container{
			padding : 20px;
		}
		dt{ margin-top: 10px;
			font-weight: bold;
			font-size: 9pt}
		/**************************/
		/** MUST BE, DON'T TOUCH **/
		table.jquery_columnSizing{
			/*N*/ table-layout: fixed;
			/*N*/ width : 100%; 
			}
		table.jquery_columnSizing tr td{
			/*N FF*/ overflow : hidden;
			}
		
		/**************************/
		/**************************/
		table{
			border-collapse:collapse;
			font : normal 7.5pt verdana;
			}
		table thead,
		table tbody tr.first {
			background-color:#def;
		}
		table tr td{
			border : 1px solid #bbb;
			vertical-align:top;
			}
		
		table col.noSelectable{
			background-color: #f0f0f0;
			color:#aaa;
		}
	</style>

	

</head>

<body>
<table class="tbHeader">
	<colgroup>
		<col />
		<col />
		<col/>
		<col class="noSelectable"/>
		<col class="noSelectable"/>
		<col />
		<col />

		<col />
	</colgroup>
	<thead>
		<tr>
			<td>column01</td>
			<td>column02</td>
			<td>column03</td>

			<td>column04</td>
			<td>column05</td>
			<td>column06</td>
			<td>column07</td>
			<td>column08</td>
		</tr>

	</thead>
	<tbody>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>

			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>

		<tr>
			<td>row</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>

			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
			<td>qqwoiruqwuor qwruryue ruyu</td>
		</tr>
	</tbody>
</table>
</body>

</html>
