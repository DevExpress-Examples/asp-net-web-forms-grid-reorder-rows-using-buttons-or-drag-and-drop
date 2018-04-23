using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;

public partial class _Default : System.Web.UI.Page {

    void UpdateGridViewButtons(ASPxGridView gridView) {
        gridView.JSProperties["cpbtMoveUp_Enabled"] = gridView.FocusedRowIndex > 0;
        gridView.JSProperties["cpbtMoveDown_Enabled"] = gridView.FocusedRowIndex < (gridView.VisibleRowCount - 1);
    }

    int GetGridViewKeyByVisibleIndex(ASPxGridView gridView, int visibleIndex) {
        return (int)gridView.GetRowValues(visibleIndex, gridView.KeyFieldName);
    }

    protected void gvProducts_Init(object sender, EventArgs e) {
        ASPxGridView gridView = sender as ASPxGridView;
        if (Session[gridView.UniqueID + "_Sort"] == null) {
            Session[gridView.UniqueID + "_Sort"] = new Dictionary<int, int>();
        }
    }

    protected void gvProducts_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e) {
        ASPxGridView gridView = sender as ASPxGridView;
        UpdateGridViewButtons(gridView);
    }

    protected void gvProducts_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) {
        ASPxGridView gridView = sender as ASPxGridView;
        if (e.Column.FieldName == "RowOrder" && e.IsGetData) {
            int rowKey = (int)e.GetListSourceFieldValue(gridView.KeyFieldName);

            Dictionary<int, int> sortIndex = (Dictionary<int, int>)Session[gridView.UniqueID + "_Sort"];

            if (!sortIndex.ContainsKey(rowKey)) {
                sortIndex[rowKey] = e.ListSourceRowIndex;
            }
            e.Value = sortIndex[rowKey];
        }
    }

    protected void gvProducts_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e) {
        ASPxGridView gridView = sender as ASPxGridView;
        string[] parameters = e.Parameters.Split('|');
        string command = parameters[0];

        Dictionary<int, int> sortIndex = (Dictionary<int, int>)Session[gridView.UniqueID + "_Sort"];


        if (command == "MOVEUP" || command == "MOVEDOWN") {
            int focusedRowKey = GetGridViewKeyByVisibleIndex(gridView, gridView.FocusedRowIndex);
            int index = sortIndex[focusedRowKey];
            int newIndex = index;

            if (command == "MOVEUP") {
                newIndex = (index == 0) ? index : index - 1;
            }
            if (command == "MOVEDOWN") {
                newIndex = (index == (gridView.VisibleRowCount - 1)) ? index : index + 1;
            }

            for (int rowIndex = 0; rowIndex < gridView.VisibleRowCount; rowIndex++) {
                int rowKey = GetGridViewKeyByVisibleIndex(gridView, rowIndex);
                if (sortIndex[rowKey] == newIndex) {
                    sortIndex[focusedRowKey] = newIndex;
                    sortIndex[rowKey] = index;
                    gridView.FocusedRowIndex = gridView.FindVisibleIndexByKeyValue(rowKey);
                    break;
                }
            }
           
        }

        if (command == "DRAGROW") {
            int draggingRowKey = int.Parse(parameters[1]);
            int targetRowKey = int.Parse(parameters[2]);

            int draggingIndex = sortIndex[draggingRowKey];
            int targetIndex = sortIndex[targetRowKey];

            int draggingDirection = (targetIndex < draggingIndex) ? 1 : -1;

            for (int rowIndex = 0; rowIndex < gridView.VisibleRowCount; rowIndex++) {
                int rowKey = GetGridViewKeyByVisibleIndex(gridView, rowIndex);

                if ((sortIndex[rowKey] > Math.Min(targetIndex, draggingIndex)) && (sortIndex[rowKey] < Math.Max(targetIndex, draggingIndex))) {
                    sortIndex[rowKey] += draggingDirection;
                }
            }

            sortIndex[draggingRowKey] = targetIndex;
            sortIndex[targetRowKey] = targetIndex + draggingDirection;

        }

        gridView.DataBind();
    }   
    
}