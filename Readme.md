<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128542189/12.2.7%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4582)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
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


<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-web-forms-grid-reorder-rows-using-buttons-or-drag-and-drop&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-web-forms-grid-reorder-rows-using-buttons-or-drag-and-drop&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
