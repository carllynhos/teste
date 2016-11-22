<%@ Page Language="C#" Inherits="Calendario_LIcitacao.pgCalendario" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<%@ Register Src="Controles/ucCalendarioPregao.ascx" TagName="ucCalendarioPregao" TagPrefix="uc1" %>
<html>
<head>
	<title>pgCalendario</title>
	<style type="text/css" media="all">@import "estilo.css";</style>
</head>
<body>
	<form id="form1" runat="server">
	<table>
		<tr><td><uc1:ucCalendarioPregao id="calendario" runat="server">
    </uc1:ucCalendarioPregao></td></tr>
	</table>
	</form>
</body>
</html>