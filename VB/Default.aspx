<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v12.2, Version=12.2.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v12.2, Version=12.2.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v12.2, Version=12.2.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGlobalEvents" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script src="jquery-1.8.3.js" type="text/javascript"></script>
	<script src="jquery-ui-1.9.2.custom.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		function InitalizejQuery() {
			$('.draggable').draggable({ helper: 'clone' });
			$('.draggable').droppable({
				activeClass: "hover",
				drop: function (event, ui) {
					var draggingRowKey = ui.draggable.find("input[type='hidden']").val();
					var targetRowKey = $(this).find("input[type='hidden']").val();
					gridView.PerformCallback("DRAGROW|" + draggingRowKey + '|' + targetRowKey);
				}
			}
			  );
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
		}

		function btMoveUp_Click(s, e) {
			gridView.PerformCallback("MOVEUP");
		}

		function btMoveDown_Click(s, e) {
			gridView.PerformCallback("MOVEDOWN");
		}

	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<table>
			<tr>
				<td rowspan="2">
					<dx:ASPxGridView ID="gvProducts" runat="server" DataSourceID="dsProducts" ClientInstanceName="gridView"
						AutoGenerateColumns="False" KeyFieldName="ProductID" OnCustomCallback="gvProducts_CustomCallback"
						OnCustomJSProperties="gvProducts_CustomJSProperties" OnCustomUnboundColumnData="gvProducts_CustomUnboundColumnData"
						OnInit="gvProducts_Init">
						<ClientSideEvents Init="gridView_Init" />
						<Columns>
							<dx:GridViewDataTextColumn FieldName="ProductID" ReadOnly="True" VisibleIndex="0">
								<EditFormSettings Visible="False" />
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn FieldName="ProductName" VisibleIndex="1">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn FieldName="UnitPrice" VisibleIndex="2">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataColumn FieldName="RowOrder" Caption=" " VisibleIndex="3" UnboundType="Integer"
								SortIndex="0" SortOrder="Ascending">
								<DataItemTemplate>
									<div class="draggable">
										<img src="Images/drag.jpg" />
										<input type="hidden" value='<%#Container.KeyValue%>' />
									</div>
								</DataItemTemplate>
							</dx:GridViewDataColumn>
						</Columns>
						<SettingsBehavior AllowSort="false" AllowFocusedRow="true" ProcessFocusedRowChangedOnServer="true" />
						<SettingsPager Mode="ShowAllRecords" />
						<ClientSideEvents Init="gridView_Init" EndCallback="gridView_EndCallback" />
					</dx:ASPxGridView>
				</td>
				<td valign="bottom">
					<dx:ASPxButton ID="btMoveUp" runat="server" Text="Move Up" Width="100%" AutoPostBack="false">
						<ClientSideEvents Click="btMoveUp_Click" />
					</dx:ASPxButton>
				</td>
			</tr>
			<tr>
				<td valign="top">
					<dx:ASPxButton ID="btMoveDown" runat="server" Text="Move Down" Width="100%" AutoPostBack="false">
						<ClientSideEvents Click="btMoveDown_Click" />
					</dx:ASPxButton>
				</td>
			</tr>
		</table>
		<asp:AccessDataSource ID="dsProducts" runat="server" DataFile="~/App_Data/nwind.mdb"
			SelectCommand="SELECT TOP 20 [ProductID], [ProductName], [UnitPrice] FROM [Products]">
		</asp:AccessDataSource>
		<dx:ASPxGlobalEvents ID="ge" runat="server">
			<ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
		</dx:ASPxGlobalEvents>
	</div>
	</form>
</body>
</html>