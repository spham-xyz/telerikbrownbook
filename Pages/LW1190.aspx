<%@ Page Language="C#" MasterPageFile="~/Controls/MasterPage.Master" AutoEventWireup="true" CodeBehind="LW1190.aspx.cs" Inherits="LW1190.Pages.LW1190" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="../css/Style.css" />
   <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
   </telerik:RadScriptManager>

   <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
      <AjaxSettings>
         <telerik:AjaxSetting AjaxControlID="multiPageMain">
            <UpdatedControls>
               <telerik:AjaxUpdatedControl ControlID="multiPageMain" LoadingPanelID="RadAjaxLoadingPanel1" />
               <telerik:AjaxUpdatedControl ControlID="tabAtty" />
               <telerik:AjaxUpdatedControl ControlID="multiPageAtty" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
         </telerik:AjaxSetting>
         <telerik:AjaxSetting AjaxControlID="multiPageAtty">
            <UpdatedControls>
               <telerik:AjaxUpdatedControl ControlID="multiPageAtty" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
         </telerik:AjaxSetting>
         <telerik:AjaxSetting AjaxControlID="SqlCommittee">
            <UpdatedControls>
               <telerik:AjaxUpdatedControl ControlID="grdCommittee" />
            </UpdatedControls>
         </telerik:AjaxSetting>
         <telerik:AjaxSetting AjaxControlID="SqlSalary">
            <UpdatedControls>
               <telerik:AjaxUpdatedControl ControlID="grdSalary" />
            </UpdatedControls>
         </telerik:AjaxSetting>
         <%--         
         <telerik:AjaxSetting AjaxControlID="rcbType">
            <UpdatedControls>
               <telerik:AjaxUpdatedControl ControlID="txtIPLevel"></telerik:AjaxUpdatedControl>
               <telerik:AjaxUpdatedControl ControlID="lblIPLevel"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
         </telerik:AjaxSetting>
         --%>
      </AjaxSettings>
      
      <%-- TESTING 
      <ClientEvents OnRequestStart="RequestStart" OnResponseEnd="ResponseEnd" />
      --%>
   </telerik:RadAjaxManager>
   <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Transparency="50">
   </telerik:RadAjaxLoadingPanel>
   <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>

   <div id="divMain" style="margin-top: -5px;">

      <telerik:RadTabStrip ID="tabMain" runat="server" SelectedIndex="0" MultiPageID="multiPageMain" Skin="Outlook" OnClientTabSelecting="tabMain_OnTabSelecting">
         <Tabs>
            <telerik:RadTab runat="server" Text="Attorney Search" PageViewID="PageAtty" Selected="True">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Committee Master Maintenance" PageViewID="PageCommitteeMstr">
            </telerik:RadTab>
         </Tabs>
      </telerik:RadTabStrip>
      <telerik:RadMultiPage ID="multiPageMain" runat="server" SelectedIndex="0" Width="850">
         <telerik:RadPageView ID="PageAtty" runat="server" Selected="True">

            <div>
               <telerik:RadGrid ID="grdAtty" runat="server" DataSourceID="SqlAtty" EnableLinqExpressions="false"
                  AutoGenerateColumns="False" CellSpacing="0" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" Skin="Vista" CssClass="hidePageSelector"
                  OnItemCommand="grdAtty_ItemCommand">
                  <%--Make RadGrid Filter Case Insensitive--%>
                  <GroupingSettings CaseSensitive="false" />
                  <ClientSettings EnableRowHoverStyle="true">
                     <ClientEvents OnPopUpShowing="PopUpShowing" />
                     <Selecting AllowRowSelect="true" />
                  </ClientSettings>
                  <SelectedItemStyle CssClass="selectedRow" />
                  <MasterTableView EditMode="PopUp" DataKeyNames="Atty ID" CommandItemDisplay="Bottom" InsertItemPageIndexAction="ShowItemOnCurrentPage" PageSize="5">
                     <HeaderStyle Font-Bold="false" />
                     <%-- 
                     http://www.telerik.com/help/aspnet-ajax/grid-custom-edit-forms.html 
                     http://demos.telerik.com/aspnet-ajax/grid/examples/dataediting/templateformupdate/defaultcs.aspx   
                     --%>
                     <EditFormSettings EditFormType="Template" InsertCaption="<b><font color=#0d4a76>Add New Record</font></b>" PopUpSettings-Width="550px">
                        <FormTemplate>
                           <div style="padding: 10px; border-style: solid; border-width: 1px; border-color: dodgerblue">
                              <telerik:RadGrid ID="grdNewAtty" runat="server" AutoGenerateColumns="False"
                                 OnItemCommand="grdNewAtty_ItemCommand"
                                 DataSourceID="SqlNewAtty" AllowFilteringByColumn="True" AllowPaging="True"
                                 AllowSorting="True" Skin="Outlook" CssClass="hidePageSelector">
                                 <GroupingSettings CaseSensitive="false" />
                                 <MasterTableView PageSize="5">
                                    <AlternatingItemStyle CssClass="altRow" />
                                    <Columns>
                                       <telerik:GridTemplateColumn UniqueName="rbNewAttyTemplateColumn" AllowFiltering="False" HeaderText="Select" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                          <ItemTemplate>
                                             <asp:RadioButton ID="rbNewAtty" OnCheckedChanged="rbNewAtty_CheckedChanged" AutoPostBack="True" runat="server" />
                                          </ItemTemplate>
                                       </telerik:GridTemplateColumn>

                                       <telerik:GridBoundColumn DataField="Atty ID" HeaderText="Atty ID" SortExpression="Atty ID" UniqueName="AttyID" FilterControlWidth="35px" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" HeaderStyle-Width="75px" ShowFilterIcon="False">
                                       </telerik:GridBoundColumn>
                                       <telerik:GridBoundColumn DataField="Loc" HeaderText="Location" SortExpression="Loc" UniqueName="Loc" AllowFiltering="False" Display="False">
                                       </telerik:GridBoundColumn>
                                       <telerik:GridBoundColumn DataField="Last Name" HeaderText="Last Name" SortExpression="Last Name" UniqueName="LastName" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                       </telerik:GridBoundColumn>
                                       <telerik:GridBoundColumn DataField="First Name" HeaderText="First Name" SortExpression="First Name" UniqueName="FirstName" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                       </telerik:GridBoundColumn>
                                       <telerik:GridBoundColumn DataField="Title" HeaderText="Title" SortExpression="Title" UniqueName="Title" AllowFiltering="false">
                                       </telerik:GridBoundColumn>

                                       <telerik:GridBoundColumn DataField="TitleCode" SortExpression="TitleCode" UniqueName="TitleCode" HeaderText="Title Code" AllowFiltering="false" Display="false">
                                       </telerik:GridBoundColumn>

                                    </Columns>
                                 </MasterTableView>
                              </telerik:RadGrid>
                              <div style="padding-top: 8px;">
                                 <asp:ImageButton ID="btnAttySave" runat="server" ImageUrl="~/Images/update.gif" CommandName="PerformInsert" ToolTip="Add New" Visible="false" />
                                 <asp:ImageButton ID="btnAttyCancel" runat="server" ImageUrl="~/Images/cancel.gif" CommandName="Cancel" ToolTip="Cancel" />
                              </div>
                           </div>
                        </FormTemplate>
                     </EditFormSettings>

                     <%--
                     <EditFormSettings PopUpSettings-Width="250px" PopUpSettings-Height="250px" PopUpSettings-Modal="True" CaptionFormatString="<b><font color=#0d4a76>Edit Client:&nbsp;&nbsp;{0}</font></b>"
                        CaptionDataField="Client" InsertCaption="<b><font color=#0d4a76>Add New Record</font></b>" PopUpSettings-ShowCaptionInEditForm="False">
                        <EditColumn ButtonType="ImageButton" />
                        <FormTableButtonRowStyle HorizontalAlign="Left"></FormTableButtonRowStyle>
                        <FormStyle BackColor="#EEF2EA" Width="100%"></FormStyle>
                        <FormTableStyle GridLines="None" CellPadding="2" CellSpacing="0"></FormTableStyle>
                        <PopUpSettings Modal="True" Width="750px" ShowCaptionInEditForm="False"></PopUpSettings>
                     </EditFormSettings>
                     --%>
                     <%--
                     <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                     <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        <HeaderStyle Width="20px"></HeaderStyle>
                     </RowIndicatorColumn>
                     <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        <HeaderStyle Width="20px"></HeaderStyle>
                     </ExpandCollapseColumn>
                     --%>
                     <AlternatingItemStyle CssClass="altRow" />
                     <CommandItemSettings ShowRefreshButton="false" />
                     <%--
                     <CommandItemTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                           <tr>
                              <td align="left">
                                 <asp:LinkButton ID="btnAddRecord" runat="server" ForeColor="#00247d" CssClass="Btn" CommandName="InitInsert" Text="Add"></asp:LinkButton>
                              </td>
                           </tr>
                        </table>
                     </CommandItemTemplate>
                     <NoRecordsTemplate>
                        No data available.  
                     </NoRecordsTemplate>
                     --%>
                     <Columns>
                        <telerik:GridTemplateColumn UniqueName="rbTemplateColumn" AllowFiltering="false" HeaderText="Select" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                           <FilterTemplate>
                              Clear filters
                              <asp:ImageButton ID="btnShowAll" runat="server" ImageUrl="~/Images/filterCancel.gif" AlternateText="Show All"
                                 ToolTip="Show All" OnClick="btnShowAll_Click" Style="vertical-align: middle" />
                           </FilterTemplate>
                           <ItemTemplate>
                              <asp:RadioButton ID="rbAtty" OnCheckedChanged="rbAtty_CheckedChanged" AutoPostBack="True" runat="server" />
                           </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Atty ID" HeaderText="Atty ID" SortExpression="Atty ID" UniqueName="AttyID" FilterControlWidth="35px" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" HeaderStyle-Width="75px" ShowFilterIcon="False">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Last Name" HeaderText="Last Name" SortExpression="Last Name" UniqueName="LastName" FilterControlWidth="110px" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="First Name" HeaderText="First Name" SortExpression="First Name" UniqueName="FirstName" FilterControlWidth="110px" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Title" HeaderText="Title" SortExpression="Title" UniqueName="Title" AllowFiltering="false">
                        </telerik:GridBoundColumn>
                        <%--
                        <telerik:GridEditCommandColumn UniqueName="EditColumn">
                        </telerik:GridEditCommandColumn>
                        --%>
                        <telerik:GridButtonColumn ConfirmText="Delete attorney and related \ncommittee(s) & salary info?" ConfirmDialogType="RadWindow"
                           ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                           <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                        </telerik:GridButtonColumn>

                     </Columns>
                  </MasterTableView>
               </telerik:RadGrid>
               <br />

               <asp:SqlDataSource ID="SqlNewAtty" runat="server"
                  ConnectionString="<%$ ConnectionStrings:ConnectionString_Data %>"
                  SelectCommand="uspLW1190_NewAttys" SelectCommandType="StoredProcedure">
                  <SelectParameters>
                     <asp:Parameter DefaultValue="A" Name="AttyType" Type="Char" />
                  </SelectParameters>
               </asp:SqlDataSource>
               <asp:SqlDataSource ID="SqlAtty" runat="server"
                  ConnectionString="<%$ ConnectionStrings:ConnectionString_Data %>"
                  SelectCommand="uspLW1190_Attys" SelectCommandType="StoredProcedure">
                  <SelectParameters>
                     <asp:Parameter DefaultValue="A" Name="AttyType" Type="Char" />
                  </SelectParameters>
               </asp:SqlDataSource>
            </div>

         </telerik:RadPageView>

         <telerik:RadPageView ID="PageCommitteeMstr" runat="server">
            <telerik:RadGrid ID="grdCommMstr" runat="server" DataSourceID="SqlCommMstr" PageSize="24"
               OnItemCommand="grdCommMstr_ItemCommand"
               AutoGenerateColumns="false" AllowAutomaticDeletes="true" AllowAutomaticInserts="true" AllowAutomaticUpdates="true"
               AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" Skin="Vista" CssClass="hidePageSelector">
               <GroupingSettings CaseSensitive="false" />
               <ClientSettings EnableRowHoverStyle="true">
                  <%--<Selecting AllowRowSelect="True" />--%>
               </ClientSettings>
               <SelectedItemStyle CssClass="selectedRow" />
               <MasterTableView CommandItemDisplay="Bottom" InsertItemDisplay="Bottom" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="cnum" EditMode="InPlace">
                  <AlternatingItemStyle CssClass="altRow" />
                  <CommandItemSettings ShowRefreshButton="false" />
                  <Columns>

                     <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditColumn">
                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                     </telerik:GridEditCommandColumn>

                     <telerik:GridTemplateColumn DataField="ccode" HeaderText="Code" SortExpression="ccode"
                        UniqueName="CodeCommMstr" AutoPostBackOnFilter="true" FilterControlWidth="75px" ShowFilterIcon="false">
                        <ItemTemplate>
                           <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ccode") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                           <telerik:RadTextBox ID="tbCode" runat="server" MaxLength="10" Text='<%# Bind("ccode") %>' Width="100"></telerik:RadTextBox>
                           <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="tbCode" ErrorMessage="Committee Code" SetFocusOnError="true" Text="*Required" Display="Dynamic"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                     </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn DataField="cdescription" HeaderText="Description" SortExpression="cdescription"
                        UniqueName="DescrpCommMstr" AutoPostBackOnFilter="true" FilterControlWidth="75px" ShowFilterIcon="false">
                        <ItemTemplate>
                           <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("cdescription") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                           <telerik:RadTextBox ID="tbDescription" runat="server" MaxLength="50" Text='<%# Bind("cdescription") %>' Width="400"></telerik:RadTextBox>
                           <asp:RequiredFieldValidator ID="rfvDescription" runat="server"  ControlToValidate="tbDescription" ErrorMessage="Committee Description" SetFocusOnError="true" Text="*Required" Display="Dynamic"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                     </telerik:GridTemplateColumn>
                     <%-- 
                     <telerik:GridBoundColumn DataField="ccode" HeaderText="Code" SortExpression="ccode"
                        UniqueName="CodeCommMstr" AutoPostBackOnFilter="true" FilterControlWidth="75px" ShowFilterIcon="false" MaxLength="10">
                     </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="cdescription" HeaderText="Description" SortExpression="cdescription" UniqueName="DescrpCommMstr"
                        AutoPostBackOnFilter="true" FilterControlWidth="250px" ShowFilterIcon="false" MaxLength="50">
                     </telerik:GridBoundColumn>
                     --%>
                     <telerik:GridButtonColumn ConfirmText="Delete selected committee\n&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;from all attorneys?" ConfirmDialogType="RadWindow"
                        ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                     </telerik:GridButtonColumn>

                  </Columns>
               </MasterTableView>
            </telerik:RadGrid>
            <br />

            <asp:SqlDataSource ID="SqlCommMstr" runat="server"
               ConnectionString="<%$ ConnectionStrings:ConnectionString_Data %>"
               SelectCommand="uspLW1190_CRUDCommitteeMaster" SelectCommandType="StoredProcedure"
               InsertCommand="uspLW1190_CRUDCommitteeMaster" InsertCommandType="StoredProcedure"
               UpdateCommand="uspLW1190_CRUDCommitteeMaster" UpdateCommandType="StoredProcedure"
               DeleteCommand="uspLW1190_CRUDCommitteeMaster" DeleteCommandType="StoredProcedure">
               <SelectParameters>
                  <asp:Parameter Name="action" Type="Char" DefaultValue="R" />
               </SelectParameters>
               <InsertParameters>
                  <asp:Parameter Name="action" Type="Char" DefaultValue="C" />
                  <asp:Parameter Name="ccode" Type="String" />
                  <asp:Parameter Name="cdescription" Type="String" />
               </InsertParameters>
               <UpdateParameters>
                  <asp:Parameter Name="action" Type="Char" DefaultValue="U" />
                  <asp:Parameter Name="ccode" Type="String" />
                  <asp:Parameter Name="cdescription" Type="String" />
                  <asp:Parameter Name="cnum" Type="Int32" />
               </UpdateParameters>
               <DeleteParameters>
                  <asp:Parameter Name="action" Type="Char" DefaultValue="D" />
                  <asp:Parameter Name="cnum" Type="Int32" />
               </DeleteParameters>
            </asp:SqlDataSource>
         </telerik:RadPageView>
      </telerik:RadMultiPage>

   </div>


   <div id="divAtty" style="margin-top: -10px;">
      <telerik:RadTabStrip ID="tabAtty" SelectedIndex="0" runat="server" MultiPageID="multiPageAtty" Skin="Outlook">
         <Tabs>
            <telerik:RadTab runat="server" Text="Personal" PageViewID="PagePersonal" Selected="True" />
            <telerik:RadTab runat="server" Text="Committee" PageViewID="PageCommittees" />
            <telerik:RadTab runat="server" Text="Salary" PageViewID="PageSalary" />
         </Tabs>
      </telerik:RadTabStrip>
      <telerik:RadMultiPage ID="multiPageAtty" runat="server" SelectedIndex="1" Width="838"
         Style="font: status-bar; padding: 5px; border-style: solid; border-width: 1px; border-color: #abbbe8">
         <telerik:RadPageView ID="PagePersonal" runat="server" Selected="True">
            <table>
               <tr>
                  <td>Full Name</td>
                  <td colspan="4">
                     <telerik:RadTextBox ID="txtFullName" runat="server" ReadOnly="true" Width="230px" BackColor="#EFEFEF"></telerik:RadTextBox></td>
                  <td style="width: 5px;"></td>
                  <td>Last Modified</td>
                  <td>
                     <telerik:RadDatePicker runat="server" ID="txtLastModified" Enabled="False" Width="100px" BackColor="#EFEFEF" MinDate="1900-01-01">
                        <DateInput runat="server" BackColor="#EFEFEF"></DateInput>
                     </telerik:RadDatePicker>
                  </td>
               </tr>
               <tr>
                  <td>Atty ID</td>
                  <td>
                     <telerik:RadTextBox ID="txtAttyID" runat="server" ReadOnly="true" BackColor="#EFEFEF"></telerik:RadTextBox></td>
                  <td style="width: 5px;"></td>
                  <td>Location </td>
                  <td>
                     <telerik:RadTextBox ID="txtLocation" runat="server" ReadOnly="true" BackColor="#EFEFEF"></telerik:RadTextBox></td>
                  <td style="width: 5px;"></td>
                  <td>Modified by</td>
                  <td>
                     <telerik:RadTextBox ID="txtModifyBy" runat="server" ReadOnly="true" BackColor="#EFEFEF"></telerik:RadTextBox></td>
               </tr>
               <tr>
                  <td>Units</td>
                  <td>
                     <telerik:RadNumericTextBox ID="txtUnits" runat="server" NumberFormat-DecimalDigits="0" TabIndex="1"></telerik:RadNumericTextBox></td>
                  <td></td>
                  <td>Bonus</td>
                  <td>
                     <telerik:RadNumericTextBox ID="txtBonus" runat="server" TabIndex="50"></telerik:RadNumericTextBox></td>
                  <td></td>
                  <td>Departure?</td>
                  <td>
                     <%--<telerik:RadComboBox ID="rcbDeparture" runat="server" Width="50px" AutoPostBack="true" OnClientSelectedIndexChanged="OnDepartureChanged" OnSelectedIndexChanged="rcbDeparture_SelectedIndexChanged" TabIndex="90">--%>
                     <telerik:RadComboBox ID="rcbDeparture" runat="server" Width="50px" OnClientSelectedIndexChanged="OnDepartureChanged" TabIndex="90">
                        <Items>
                           <telerik:RadComboBoxItem Text="No" Value="N" />
                           <telerik:RadComboBoxItem Text="Yes" Value="Y" />
                        </Items>
                     </telerik:RadComboBox>
                  </td>
               </tr>
               <tr>
                  <td>Adjusted Units</td>
                  <td>
                     <telerik:RadNumericTextBox ID="txtAdjUnits" runat="server" NumberFormat-DecimalDigits="0" TabIndex="10"></telerik:RadNumericTextBox></td>
                  <td></td>
                  <td>Date of Birth</td>
                  <td>
                     <telerik:RadDatePicker ID="txtDOB" runat="server" MinDate="1/1/1900" MaxDate="12/31/2099" Width="100px" TabIndex="60"></telerik:RadDatePicker>
                  </td>
                  <td></td>
                  <td>Departure Date</td>
                  <td>
                     <telerik:RadDatePicker ID="txtDepartDate" runat="server" MinDate="1/1/1900" MaxDate="12/31/2099" Width="100px" TabIndex="100"></telerik:RadDatePicker>
                     <asp:RequiredFieldValidator ID="rfvDepartDate" runat="server" ControlToValidate="txtDepartDate" ErrorMessage="Departure Date" SetFocusOnError="true" Text="*Required" Enabled="false"></asp:RequiredFieldValidator></td>
               </tr>
               <tr>
                  <td rowspan="2">Unit Value</td>
                  <td rowspan="2">
                     <telerik:RadNumericTextBox ID="txtUnitValue" runat="server" TabIndex="20"></telerik:RadNumericTextBox></td>
                  <td rowspan="2"></td>
                  <td>Type</td>
                  <td>
                     <telerik:RadComboBox ID="rcbType" runat="server" OnSelectedIndexChanged="rcbType_SelectedIndexChanged" AutoPostBack="True" Width="85px" TabIndex="70">
                        <Items>
                           <telerik:RadComboBoxItem Text="- Select -" Value="" />
                           <telerik:RadComboBoxItem Text="Equity" Value="E" />
                           <telerik:RadComboBoxItem Text="Income" Value="I" />
                           <telerik:RadComboBoxItem Text="Counsel" Value="C" />
                           <telerik:RadComboBoxItem Text="Of Counsel" Value="O" />
                           <telerik:RadComboBoxItem Text="Associate" Value="A" />
                        </Items>
                     </telerik:RadComboBox>
                     <asp:CustomValidator
                        ID="valType"
                        runat="server"
                        ClientValidationFunction="requiredType"
                        ErrorMessage="Input Type" 
                        Text="*Required"
                        OnServerValidate="valType_ServerValidate">
                     </asp:CustomValidator></td>
                  <td rowspan="2"></td>
                  <td rowspan="2">Class</td>
                  <td rowspan="2">
                     <telerik:RadMaskedTextBox runat="server" ID="txtClass" Width="45px" Mask="####" PromptChar='' TabIndex="110"></telerik:RadMaskedTextBox></td>
               </tr>
               <tr>
                  <td>
                     <asp:Label ID="lblIPLevel" runat="server" Text="IP Level" Style="display: none;"></asp:Label></td>
                  <td>
                     <telerik:RadNumericTextBox ID="txtIPLevel" runat="server" Width="40px" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Style="display: none;" TabIndex="75"></telerik:RadNumericTextBox>
                     <asp:RequiredFieldValidator ID="rfvIPLevel" runat="server" ControlToValidate="txtIPLevel" ErrorMessage="IP Level" SetFocusOnError="true" Text="*Required" Enabled="false"></asp:RequiredFieldValidator>
                  </td>
               </tr>
               <tr>
                  <td>Pre-Fund Compensation</td>
                  <td>
                     <telerik:RadNumericTextBox ID="txtPreFundComp" runat="server" TabIndex="30"></telerik:RadNumericTextBox></td>
                  <td></td>
                  <td>Rank</td>
                  <td>
                     <telerik:RadComboBox ID="rcbRank" runat="server" Width="125px" TabIndex="80">
                        <Items>
                           <telerik:RadComboBoxItem Text="Print & Rank" Value="1" />
                           <telerik:RadComboBoxItem Text="Print/No Rank" Value="2" />
                           <telerik:RadComboBoxItem Text="No Print/No Rank" Value="3" />
                        </Items>
                     </telerik:RadComboBox>
                  </td>
                  <td></td>
                  <td><asp:Label ID="lblEvalClass" runat="server" Text="Evaluation Class" Style="display: none;"></asp:Label></td>
                  <td><telerik:RadMaskedTextBox ID="txtEvalClass" runat="server" Width="45px" Mask="####" PromptChar='' TabIndex="120" Style="display: none;"></telerik:RadMaskedTextBox></td>
               </tr>
               <tr>
                  <td>Partner Fund</td>
                  <td>
                     <telerik:RadNumericTextBox ID="txtPrnFund" runat="server" TabIndex="40"></telerik:RadNumericTextBox></td>
                  <td></td>
                  <td>Principal Practice Group</td>
                  <td>
                     <telerik:RadTextBox ID="txtPrinPracGroup" runat="server" ReadOnly="true" BackColor="#EFEFEF"></telerik:RadTextBox></td>
                  <td></td>
                  <td><asp:Label ID="lblAdminDate" runat="server" Text="Equity Partner Admission Date" Style="display: none;"></asp:Label></td>
                  <td>
                     <telerik:RadDatePicker ID="txtAdminDate" runat="server" Enabled="false" MinDate="1/1/1900" MaxDate="12/31/2099" Width="100px" BackColor="#EFEFEF" Style="display: none;">
                        <DateInput runat="server" BackColor="#EFEFEF"></DateInput>
                     </telerik:RadDatePicker>
                  </td>
               </tr>
               <tr>
                  <td colspan="3"></td>
                  <td>Bar</td>
                  <td colspan="4">
                     <telerik:RadTextBox ID="txtBar" runat="server" ReadOnly="true" Width="475px" TextMode="MultiLine" BackColor="#EFEFEF"></telerik:RadTextBox></td>
               </tr>
               <tr>
                  <td>
                     <asp:Label ID="lblComment" runat="server" Text="Comments"></asp:Label></td>
                  <td colspan="7">
                     <telerik:RadTextBox ID="txtComment" runat="server" MaxLength="150" Height="50px" Width="740px" TabIndex="130"></telerik:RadTextBox></td>
               </tr>
               <tr>
                  <td colspan="8">
                     <asp:ImageButton ID="btnDtlUpdate" runat="server" ImageUrl="~/Images/update.gif" AlternateText="Update Personal"
                        ToolTip="Update Personal" Style="vertical-align: middle" Visible="false" OnClick="btnDtlUpdate_Click" OnClientClick="OnDtlUpdate" /><asp:ImageButton ID="btnDtlCancel" runat="server" ImageUrl="~/Images/cancel.gif" AlternateText="Undo Changes"
                           ToolTip="Undo Changes" Style="vertical-align: middle" Visible="false" OnClick="btnDtlCancel_Click" /></td>
               </tr>
            </table>
         </telerik:RadPageView>
         <telerik:RadPageView ID="PageCommittees" runat="server">
            <telerik:RadGrid ID="grdCommittee" runat="server" DataSourceID="SqlCommittee"
               OnItemDataBound="grdCommittee_ItemDataBound"
               OnItemCommand="grdCommittee_ItemCommand"
               AutoGenerateColumns="false" AllowAutomaticDeletes="true"
               Skin="Vista" CssClass="hidePageSelector">
               <ClientSettings EnableRowHoverStyle="true"></ClientSettings>
               <SelectedItemStyle CssClass="selectedRow" />
               <MasterTableView InsertItemDisplay="Bottom" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="tkinit,cnum" EditMode="InPlace">
                  <AlternatingItemStyle CssClass="altRow" />
                  <CommandItemSettings ShowRefreshButton="false" />
                  <Columns>
                     <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditColumn">
                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                     </telerik:GridEditCommandColumn>
                     <%-- For update --> editedItem.SavedOldValues: dictionary holds the old values --%>
                     <telerik:GridBoundColumn DataField="tkinit" HeaderText="Atty Id" SortExpression="tkinit" UniqueName="tkinit" ColumnEditorID="edit_tkinit" Display="false"></telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="cnum" HeaderText="Committee Code" SortExpression="cnum" UniqueName="cnum" ColumnEditorID="edit_cnum" Display="false"></telerik:GridBoundColumn>
                     <telerik:GridTemplateColumn DataField="ccode" UniqueName="ComCodeEdit" HeaderText="Committee Code">
                        <ItemTemplate><%# Eval("ccode") %></ItemTemplate>
                        <EditItemTemplate>
                           <telerik:RadComboBox ID="rcbComCodeEdit" runat="server" Width="400px" MaxHeight="245px">
                              <ItemTemplate>
                                 <ul class="hiliterow">
                                    <li class="col1"><%# Eval("ccode") %></li>
                                    <li class="col2"><%# Eval("cdescription") %></li>
                                 </ul>
                              </ItemTemplate>
                           </telerik:RadComboBox>
                        </EditItemTemplate>
                     </telerik:GridTemplateColumn>
                     <telerik:GridBoundColumn DataField="cdescription" HeaderText="Description" SortExpression="cdescription" UniqueName="cdescription" ColumnEditorID="edit_cdescription" ReadOnly="True"></telerik:GridBoundColumn>
                     <telerik:GridTemplateColumn DataField="cchair" UniqueName="ChairEdit" HeaderText="Dept Chair">
                        <ItemTemplate><%# Eval("cchair") %></ItemTemplate>
                        <EditItemTemplate>
                           <telerik:RadComboBox ID="rcbChairEdit" runat="server" Width="50px">
                              <Items>
                                 <telerik:RadComboBoxItem Text="No" Value="N" />
                                 <telerik:RadComboBoxItem Text="Yes" Value="Y" />
                              </Items>
                           </telerik:RadComboBox>
                        </EditItemTemplate>
                     </telerik:GridTemplateColumn>
                     <telerik:GridButtonColumn ConfirmText="Delete selected committee?" ConfirmDialogType="RadWindow"
                        ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete"
                        UniqueName="DeleteColumn">
                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                     </telerik:GridButtonColumn>
                  </Columns>
               </MasterTableView>
            </telerik:RadGrid><br />
            <asp:SqlDataSource ID="SqlCommittee" runat="server"
               ConnectionString="<%$ ConnectionStrings:ConnectionString_Data %>"
               SelectCommand="uspLW1190_CRUDCommittee" SelectCommandType="StoredProcedure"
               DeleteCommand="uspLW1190_CRUDCommittee" DeleteCommandType="StoredProcedure">
               <SelectParameters>
                  <asp:Parameter Name="action" Type="Char" DefaultValue="R" />
                  <asp:Parameter Name="tkinit" Type="String" />
               </SelectParameters>
               <DeleteParameters>
                  <asp:Parameter Name="action" Type="Char" DefaultValue="D" />
                  <asp:Parameter Name="tkinit" Type="String" />
                  <asp:Parameter Name="cnum" Type="Int32" />
               </DeleteParameters>
            </asp:SqlDataSource>
         </telerik:RadPageView>
         <telerik:RadPageView ID="PageSalary" runat="server">
            <telerik:RadGrid ID="grdSalary" runat="server" DataSourceID="SqlSalary" AutoGenerateColumns="false"
               OnItemCreated="grdSalary_ItemCreated" OnItemDataBound="grdSalary_ItemDataBound" OnItemCommand="grdSalary_ItemCommand"
               AllowAutomaticDeletes="true"
               Skin="Vista" CssClass="hidePageSelector">
               <ClientSettings EnableRowHoverStyle="true"></ClientSettings>
               <SelectedItemStyle CssClass="selectedRow" />
               <MasterTableView InsertItemDisplay="Bottom" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="TimeKeep,Seq_no" EditMode="InPlace">
                  <AlternatingItemStyle CssClass="altRow" />
                  <CommandItemSettings ShowRefreshButton="false" />
                  <Columns>
                     <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditColumn">
                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                     </telerik:GridEditCommandColumn>

                     <%-- For update --> editedItem.SavedOldValues: dictionary holds the old values --%>
                     <telerik:GridBoundColumn ColumnEditorID="edit_TimeKeep" DataField="TimeKeep" Display="false" HeaderText="Atty Id" SortExpression="TimeKeep" UniqueName="TimeKeep"></telerik:GridBoundColumn>
                     <telerik:GridBoundColumn ColumnEditorID="edit_Seq_no" DataField="Seq_no" Display="false" HeaderText="Seq No." SortExpression="Seq_no" UniqueName="Seq_no"></telerik:GridBoundColumn>
                     <telerik:GridNumericColumn ColumnEditorID="edit_LocalSalary" DataField="LocalSalary" DataFormatString="{0:N2}" HeaderText="Salary" UniqueName="LocalSalary" />
                     <telerik:GridTemplateColumn DataField="curcode" UniqueName="CurrCodeEdit" HeaderText="Currency Code">
                        <ItemTemplate><%# Eval("CurrCode") %></ItemTemplate>
                        <EditItemTemplate>
                           <telerik:RadComboBox ID="rcbCurrCodeEdit" runat="server" Width="250px">
                              <ItemTemplate>
                                 <ul class="hiliterow">
                                    <li class="colA"><%# Eval("curcode") %></li>
                                    <li class="colB"><%# Eval("curdesc") %></li>
                                 </ul>
                              </ItemTemplate>
                           </telerik:RadComboBox>
                        </EditItemTemplate>
                     </telerik:GridTemplateColumn>
                     <telerik:GridDateTimeColumn DataField="EffectiveDate" HeaderText="Effective Date" UniqueName="EffectiveDate" DataType="System.DateTime" DataFormatString="{0:d}" EditDataFormatString="MM/dd/yyyy" PickerType="DatePicker" />
                     <telerik:GridBoundColumn DataField="Comment" HeaderText="Comment" SortExpression="Comment" UniqueName="Comment" ColumnEditorID="edit_Comment" MaxLength="255" />
                     <telerik:GridButtonColumn ConfirmText="Delete selected salary?" ConfirmDialogType="RadWindow"
                        ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete"
                        UniqueName="DeleteColumn">
                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                     </telerik:GridButtonColumn>
                  </Columns>
               </MasterTableView>
            </telerik:RadGrid><br />
            <asp:SqlDataSource ID="SqlSalary" runat="server"
               ConnectionString="<%$ ConnectionStrings:ConnectionString_Data %>"
               SelectCommand="uspLW1190_CRUDSalary" SelectCommandType="StoredProcedure"
               DeleteCommand="uspLW1190_CRUDSalary" DeleteCommandType="StoredProcedure">
               <SelectParameters>
                  <asp:Parameter Name="action" Type="Char" DefaultValue="R" />
                  <asp:Parameter Name="TimeKeep" Type="String" />
               </SelectParameters>
               <DeleteParameters>
                  <asp:Parameter Name="action" Type="Char" DefaultValue="D" />
                  <asp:Parameter Name="TimeKeep" Type="String" />
                  <asp:Parameter Name="Seq_no" Type="Int32" />
               </DeleteParameters>
            </asp:SqlDataSource>
         </telerik:RadPageView>
      </telerik:RadMultiPage>

   </div>
   <br />
   <asp:Button ID="btnExportExcel" runat="server" Font-Bold="False" Text="Export Excel"
   Width="100px" OnClick="btnExportExcel_Click" />


   <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Missing required value(s):" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" Font-Size="Small" />--%>

   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <script src="../Scripts/JScript.js" type="text/javascript"></script>
   </telerik:RadCodeBlock>
</asp:Content>
