using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class _Default : System.Web.UI.Page
{
    void UpdateGridViewButtons(ASPxGridView gridView)
    {
        gridView.JSProperties["cpbtMoveUp_Enabled"] = gridView.FocusedRowIndex > 0;
        gridView.JSProperties["cpbtMoveDown_Enabled"] = gridView.FocusedRowIndex < (gridView.VisibleRowCount - 1);
    }

    int GetGridViewKeyByVisibleIndex(ASPxGridView gridView, int visibleIndex)
    {
        return (int)gridView.GetRowValues(visibleIndex, gridView.KeyFieldName);
    }

    protected void gvProducts_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView gridView = sender as ASPxGridView;
        UpdateGridViewButtons(gridView);
    }

    private int GetKeyIDBySortIndex(ASPxGridView gridView, int sortIndex)
    {
        dsHelper.SelectParameters["DisplayOrder"].DefaultValue = sortIndex.ToString();
        int rowKey = (int)(dsHelper.Select(DataSourceSelectArguments.Empty) as System.Data.DataView)[0][gridView.KeyFieldName];
        return rowKey;
    }

    private void UpdateSortIndex(int rowKey, int sortIndex)
    {
        dsProducts.UpdateParameters["ProductID"].DefaultValue = rowKey.ToString();
        dsProducts.UpdateParameters["DisplayOrder"].DefaultValue = sortIndex.ToString();
        dsProducts.Update();
    }

    protected void gvProducts_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView gridView = sender as ASPxGridView;
        string[] parameters = e.Parameters.Split('|');
        string command = parameters[0];
        if (command == "MOVEUP" || command == "MOVEDOWN")
        {
            int focusedRowKey = GetGridViewKeyByVisibleIndex(gridView, gridView.FocusedRowIndex);
            int index = (int)gridView.GetRowValues(gridView.FocusedRowIndex, "DisplayOrder");
            int newIndex = index;
            if (command == "MOVEUP")
            {
                newIndex = (index == 0) ? index : index - 1;
            }
            if (command == "MOVEDOWN")
            {
                newIndex = (index == gridView.VisibleRowCount) ? index : index + 1;
            }
            int rowKey = GetKeyIDBySortIndex(gridView, newIndex);
            UpdateSortIndex(focusedRowKey, newIndex);
            UpdateSortIndex(rowKey, index);
            gridView.FocusedRowIndex = gridView.FindVisibleIndexByKeyValue(rowKey);
        }
        if (command == "DRAGROW")
        {
            int draggingIndex = int.Parse(parameters[1]);
            int targetIndex = int.Parse(parameters[2]);
            int draggingRowKey = GetKeyIDBySortIndex(gridView, draggingIndex);
            int targetRowKey = GetKeyIDBySortIndex(gridView, targetIndex);
            int draggingDirection = (targetIndex < draggingIndex) ? 1 : -1;
            for (int rowIndex = 0; rowIndex < gridView.VisibleRowCount; rowIndex++)
            {
                int rowKey = GetGridViewKeyByVisibleIndex(gridView, rowIndex);
                int index = (int)gridView.GetRowValuesByKeyValue(rowKey, "DisplayOrder");
                if ((index > Math.Min(targetIndex, draggingIndex)) && (index < Math.Max(targetIndex, draggingIndex)))
                {
                    UpdateSortIndex(rowKey, index + draggingDirection);
                }
            }
            UpdateSortIndex(draggingRowKey, targetIndex);
            UpdateSortIndex(targetRowKey, targetIndex + draggingDirection);
        }
        gridView.DataBind();
    }
    protected void gvProducts_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.Data)
        {
            object rowOrder = e.GetValue("DisplayOrder");
            if (rowOrder != null)
                e.Row.Attributes.Add("sortOrder", rowOrder.ToString());
        }
    }
}