<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/Default.aspx) (VB: [Default.aspx](./VB/Default.aspx))
* [Default.aspx.cs](./CS/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/Default.aspx.vb))
<!-- default file list end -->
# How to reorder ASPxGridView rows using buttons or drag-and-drop


<p>This example demonstrates how to move ASPxGridView rows using buttons or jQuery Drag&Drop.</p>
<br />
<p>To keep the order of rows, it is necessary to set up an extra column to store row order indexes. Then, sort ASPxGridView by this column and deny sorting by other columns. This example stores this information in the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument3732"><u>unbound column</u></a> and additionally puts a dictionary "row key = row index" to the session. We have implemented this approach only for demo purposes. You can store this information in your DataSource.<br /><strong><br />Updated:<br /><br /></strong></p>
<p>We updated an example for v.14.2 to show how to save order information to a database and tune the ASPxGridView drag and drop appearance using the UI <a href="http://jqueryui.com/draggable/">Draggable</a> and <a href="http://jqueryui.com/droppable/">Droppable</a> plug-ins. If you need to check the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument3732">unbound column</a>  implementation, choose the second item in the version build combo box. </p>
<p> </p>
<p><strong>See also:<br /><a href="https://www.devexpress.com/Support/Center/p/T191258">T191258 - How to reorder GridView rows using buttons or drag-and-drop</a></strong><br /> <a href="https://www.devexpress.com/Support/Center/p/E1810">E1810: How to use jQuery to drag and drop items from one ASPxGridView to another</a></p>
<p><a href="https://www.devexpress.com/Support/Center/p/E3850">E3850: How to reorder ASPxTreeList sibling nodes, using buttons or drag-and-drop</a><u><br /> </u><a href="https://www.devexpress.com/Support/Center/p/E4299">E4299: How to move up or down a line a row of ASPxGridView by using external buttons</a></p>

<br/>


