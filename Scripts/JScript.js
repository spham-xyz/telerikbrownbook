var $ = $telerik.$;

//$(document).ready(function () {
//    $('[name$=rcbDeparture]').change(function () {
//        alert($(this).next().val($(this).children(':selected').text()));
//    });
//});

function tabMain_OnTabSelecting(sender, args) {
    //if (args.get_tab().get_pageViewID() == "PageCommittee")
    if (args.get_tab().get_pageViewID().indexOf('PageCommittee') !== -1)
        $('div#divAtty').hide();
    else
        $('div#divAtty').show();
}

/* Bug: txtIPLevel is shown, but disappear when hovering over it.
function OnTypeChanged(sender, args) {
    //sender.get_selectedItem().get_text()          -- current
    //args.get_item().get_text()                    -- to be selected
    var lblIP = $('[id$=lblIPLevel]');
    var txtIP = $('[id$=txtIPLevel]');

    if (args.get_item().get_value() == "I") {
        //if ($('[id$=lblIPLevel]').is(':visible') == false) {
        //    $('[id$=lblIPLevel]').show();
        //    $('[id$=txtIPLevel]').show();
        //}
        //lblIP.css({ "display": "block" });
        //txtIP.css({ "display": "block" });
    }
    else {
        //if ($('[id$=lblIPLevel]').is(':visible') == true) {
        //    $('[id$=lblIPLevel]').hide();
        //    $('[id$=txtIPLevel]').hide();
        //}
        //lblIP.css({ "display": "none" });
        //txtIP.css({ "display": "none" });
    }
}
*/

//function ClearConfirm(sender, args) {
//    $('[id$=lblConfirmation]').text('');      // id$= will match the elements that end with that text
//}

//http://techbrij.com/dynamically-enable-disable-validator-isvalid-asp-net
function OnDepartureChanged(sender, args) {
    var rfv = $('[id$=rfvDepartDate]');
    var dteDepart = $('[id$="txtDepartDate"]');

    if (sender.get_selectedItem().get_value() == "Y") {
        ValidatorEnable(rfv[0], true);
        dteDepart[0].disabled = false;
    }
    else {
        ValidatorEnable(rfv[0], false);
        //dteDepart[0].defaultValue = "";
        dteDepart[0].value = '';
        dteDepart[0].disabled = true;
    }
}
//http://www.telerik.com/help/aspnet-ajax/combobox-validate-combobox-value.html
function OnDtlUpdate() {
    var rfv = $('[id$=rfvType]');
    var cmbType = $('[id$=rcbType]');
    
    if (cmbType[0].value == '- Select -') {
        ValidatorEnable(rfv[0], true);
    }
    else {
        ValidatorEnable(rfv[0], false);
    }
}

function requiredType(sender, args) {
    var name = $('[id$=txtFullName]');      // Need to remember the brackets "[...]"
    var cmbType = $('[id$=rcbType]');
    var ipLvl = $('[id$=txtIPLevel]');
    var val = $('[id$=valType]');

    var rfv = $('[id$=rfvIPLevel]');

    // Don't want to validate on adding new attorney
    if (name[0].value != "") {
        ValidatorEnable(rfv[0], false);

        switch (cmbType[0].value) {
            case '- Select -':
                args.IsValid = false;
                val[0].innerText = "*Required";
                $('[id$=lblIPLevel]').hide();
                $('[id$=txtIPLevel]').hide();
                break;
            case 'Income':
                //if (ipLvl[0].value == "") args.IsValid = false;
            //    $('[id$=lblIPLevel]').show();
            //    $('[id$=txtIPLevel]').show();
                if (ipLvl.is(':visible') && ipLvl[0].value == "")
                    ValidatorEnable(rfv[0], true);
                break;
            default:
                val[0].innerText = "";
                args.IsValid = true;
                break;
        }
    } else {
        args.IsValid = true;
    }
}

//Center PopUp Edit Form in RadGrid
//http://www.telerik.com/help/aspnet-ajax/grid-center-popup-edit-form.html
var popUp;
function PopUpShowing(sender, eventArgs) {
    popUp = eventArgs.get_popUp();
    var gridWidth = sender.get_element().offsetWidth;
    var gridHeight = sender.get_element().offsetHeight;
    var popUpWidth = popUp.style.width.substr(0, popUp.style.width.indexOf("px"));
    var popUpHeight = popUp.style.height.substr(0, popUp.style.height.indexOf("px"));
    popUp.style.left = ((gridWidth - popUpWidth) / 2 + sender.get_element().offsetLeft).toString() + "px";
    //popUp.style.top = ((gridHeight - popUpHeight) / 2 + sender.get_element().offsetTop).toString() + "px";
    popUp.style.top = ((gridHeight - popUpHeight) / 2).toString() + "px";
}