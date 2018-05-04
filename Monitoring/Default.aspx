<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Monitoring.Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Monitoring Home</title>
</head>
<body>
    <p>Welcome</p>
    
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="nnn" runat="server"></asp:TextBox>

            <asp:DropDownList ID="ddlURL" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlURL_SelectedIndexChanged" Width="171px">
                <asp:ListItem Text="" Value="" />
            </asp:DropDownList>

        </div>
        <asp:Chart ID="LatencyChart" Type="Line" runat="server" Width="1197px">
            <series>
                <asp:Series ChartType="Line" Name="LatencySeries">
                </asp:Series>
            </series>
            <chartareas>
                <asp:ChartArea Name="LatencyChartArea">
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>

        <asp:Chart ID="StatusChart" Type="Line" runat="server" Width="1197px">
            <series>
                <asp:Series ChartType="Line" Name="StatusSeries">
                </asp:Series>
            </series>
            <chartareas>
                <asp:ChartArea Name="StatusChartArea">
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>
    </form>

</body>
</html>
