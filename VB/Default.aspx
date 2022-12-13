<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.10.4/jquery-ui.min.js"></script>
    <script src="jquery.ui.touch-punch.min.js"></script>
    <style>
        .hover {
            background-color: lightblue;
        }

        .activeHover {
            background-color: lightgray;

        }

        .ui-draggable-dragging {
            background-color: lightgreen;
            color: White;
        }
    </style>
    <script type="text/javascript">
        var states = [];

        function InitalizejQuery() {
            $('.draggable').draggable({
                helper: 'clone',
                start: function (ev, ui) {
                    var $draggingElement = $(ui.helper);
                    $draggingElement.width(gridView.GetWidth());
                }
            });
            $('.draggable').droppable({
                activeClass: "hover",
                tolerance: "intersect",
                hoverClass: "activeHover",
                drop: function (event, ui) {
                    var draggingSortIndex = ui.draggable.attr("sortOrder");
                    var targetSortIndex = $(this).attr("sortOrder");                  
                    MakeAction("DRAGROW|" + draggingSortIndex + '|' + targetSortIndex);
                }
            });
        }
        function UpdatedGridViewButtonsState(grid) {
            btMoveUp.SetEnabled(grid.cpbtMoveUp_Enabled);
            btMoveDown.SetEnabled(grid.cpbtMoveDown_Enabled);
        }

        function gridView_Init(s, e) {
            UpdatedGridViewButtonsState(s);
        }

        function gridView_EndCallback(s, e) {
            UpdatedGridViewButtonsState(s);
            NextAction();
        }

        function btMoveUp_Click(s, e) {
            MakeAction("MOVEUP");
        }

        function btMoveDown_Click(s, e) {
            MakeAction("MOVEDOWN");
        }

        function MakeAction(action) {
            if (gridView.InCallback())
                states.push(action)
            else
                gridView.PerformCallback(action)
        }

        function NextAction() {
            if (states.length != 0) {
                var currentState = states.shift();
                if (currentState == "MOVEUP" && gridView.cpbtMoveUp_Enabled)
                    gridView.PerformCallback(currentState);
                else if (currentState == "MOVEDOWN" && gridView.cpbtMoveDown_Enabled)
                    gridView.PerformCallback(currentState);
                else if (currentState.indexOf("DRAGROW") != -1)
                    gridView.PerformCallback(currentState);
                else
                    NextAction();
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td rowspan="2">
                        <dx:ASPxGridView ID="gvProducts" runat="server" OnHtmlRowPrepared="gvProducts_HtmlRowPrepared" DataSourceID="dsProducts" ClientInstanceName="gridView"
                            AutoGenerateColumns="False" KeyFieldName="ProductID" OnCustomCallback="gvProducts_CustomCallback"
                            OnCustomJSProperties="gvProducts_CustomJSProperties">
                            <ClientSideEvents Init="gridView_Init" />
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="ProductID" ReadOnly="True" VisibleIndex="0">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ProductName" VisibleIndex="1">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="DisplayOrder" Visible="false" VisibleIndex="2" SortIndex="0"
                                    SortOrder="Ascending">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="UnitPrice" ShowInCustomizationForm="True" VisibleIndex="3">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <Styles>
                                <Row CssClass="draggable"></Row>
                            </Styles>
                            <SettingsBehavior AllowSort="false" AllowFocusedRow="true" ProcessFocusedRowChangedOnServer="True" />
                            <SettingsPager Mode="ShowAllRecords" />
                            <ClientSideEvents Init="gridView_Init" EndCallback="gridView_EndCallback" />
                        </dx:ASPxGridView>
                    </td>
                    <td class="style1" style="vertical-align: bottom">
                        <dx:ASPxButton ID="btMoveUp" runat="server" Text="Up" Width="100px" AutoPostBack="false">
                            <ClientSideEvents Click="btMoveUp_Click" />
                        </dx:ASPxButton>
                    </td>
                </tr>
                <tr>
                    <td class="style1" style="vertical-align: top">
                        <dx:ASPxButton ID="btMoveDown" runat="server" Text="Down" Width="100px" AutoPostBack="false">
                            <ClientSideEvents Click="btMoveDown_Click" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
            <asp:AccessDataSource ID="dsProducts" runat="server" DataFile="~/App_Data/nwind.mdb"
                SelectCommand="SELECT TOP 10 [ProductID], [ProductName], [DisplayOrder], [UnitPrice] FROM [Products]"
                UpdateCommand="UPDATE Products SET [DisplayOrder] = ? where [ProductID] = ?">
                <UpdateParameters>
                    <asp:Parameter Name="DisplayOrder" />
                    <asp:Parameter Name="ProductID" />
                </UpdateParameters>
            </asp:AccessDataSource>
            <asp:AccessDataSource ID="dsHelper" runat="server" DataFile="~/App_Data/nwind.mdb"
                SelectCommand="SELECT [ProductID] FROM [Products] where [DisplayOrder] = ?">
                <SelectParameters>
                    <asp:Parameter Name="DisplayOrder" />
                </SelectParameters>
            </asp:AccessDataSource>
            <dx:ASPxGlobalEvents ID="ge" runat="server">
                <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
            </dx:ASPxGlobalEvents>
        </div>
    </form>
</body>
</html>