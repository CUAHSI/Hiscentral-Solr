

var agencyAward = new Array();

function deleteConfirmDialog(id) {
    $('#alert22').show();
}

function tt() {
    alert("enter");
    $("#a").removeClass("btn btn-danger");
    $("#a").addClass("greybutton");
}

//function yes() {
//    var str = "aa,bb,cc,dd,";
//    alert(str);
//    var res = str.replace(/,/g, "");
//    alert(res);
//}


/// Clear add grants popup textbox
function emptyPopup(number) {
    var count = 0;
    $('.clsAG-' + number).each(function () {

        $(this).val("");
        $(this).css({
            "border": "",
            "background": ""
        });
    });

    $('.clsAID-' + number).each(function () {
        $(this).val("");
        $(this).css({
            "border": "",
            "background": ""
        });
    });

    $('#start-' + number).each(function () {
        $(this).val("");
    });

    $('.datepicker').each(function () {
        $(this).val("");
    });

    $('.datepicker1').each(function () {
        $(this).val("");
    });

    $('#end-' + number).each(function () {
        $(this).val("");
    });
    $('.url-' + number).each(function () {
        $(this).val("");
    });
    $('.clscomplete-' + number).each(function () {
        $('.clscomplete-' + number).prop('checked', false);
    });
    return false;
}



/// Show popup for edit and delete
function EditMain(number) {


    $('.editend-' + number).datepicker({ autoclose: true }).on('changeDate', function () { $(this).datepicker('hide'); });;
    $('.editstart-' + number).datepicker({ autoclose: true }).on('changeDate', function () { $(this).datepicker('hide'); });;

    $('.check-' + number).each(function () {
        if ($(this).is(":checked")) {
            $(this).prop('checked', false);
        }
    });
    $('.selectall' + number).each(function () {
        if ($(this).is(":checked")) {
            $(this).prop('checked', false);
        }
    });
    $('#myModaledit' + number).modal('toggle');
}



/// Call when any delete checkbox checked
function checkone(number) {

    //alert('cald')
    var count = 0;
    $('.check-' + number).each(function () {
        if ($(this).is(":checked")) {
            count++;
            //$("#A2" + number).prop("disabled", false);
            //$("#A2" + number).removeClass("greybutton");
            //$("#A2" + number).addClass("btn btn-danger");

            //alert('cald')
        }
        //else {
        //    //$("#A2" + number).attr("disabled", "disabled");
        //    //$("#A2" + number).removeClass("btn btn-danger");
        //    //$("#A2" + number).addClass("greybutton");
        //    //count++;

        //}


    });

    //alert(count)

    if (count != 0) {
        $('.selectall-' + number).prop('checked', false);
        $("#A2" + number).removeClass("greybutton");
        $("#A2" + number).addClass("btn btn-danger");
        $("#A2" + number).prop("disabled", false);
    }
    else {
        $("#A2" + number).attr("disabled", "disabled");
        $("#A2" + number).removeClass("btn btn-danger");
        $("#A2" + number).addClass("greybutton");
    }


    //if ($('.selectall-'+number).is(":checked")) {
    //    $("#A2" + number).removeClass("greybutton");
    //    $("#A2" + number).addClass("btn btn-danger");
    //    $("#A2" + number).prop("disabled", false);
    //}

}



/// When header delete checked
function checkall(number) {

    var count = 0;

    if ($('.selectall-' + number).is(":checked")) {

        count = 1;

        $('.check-' + number).each(function () {
            $(this).prop('checked', true);
            $("#A2" + number).prop("disabled", false).button('refresh');
        });
    }
    else {

        count = 0;

        $('.check-' + number).each(function () {
            $(this).prop('checked', false);
            $("#A2" + number).attr("disabled", "disabled");
        });
    }

    if (count == 1) {

        $("#A2" + number).removeClass("greybutton");
        $("#A2" + number).addClass("btn btn-danger");
        $("#A2" + number).prop("disabled", false);
    }
    else {
        $("#A2" + number).attr("disabled", "disabled");
        $("#A2" + number).removeClass("btn btn-danger");
        $("#A2" + number).addClass("greybutton");
    }
}




var complete = "";
var agency = "";
var award = "";
var agencyAward = new Array();
/// Update grants
function update(number) {
    var empty1 = "N/A";
    var isValid = true;
    var tbStartDate = document.getElementsByName("startDate" + number);
    var tbEndDate = document.getElementsByName("endDate" + number);
    //alert(tbStartDate.length);
    for (var i = 0; i < tbStartDate.length; i++) {
        var txtStartDate = tbStartDate[i].value;
        var txtEndDate = tbEndDate[i].value;
        if (new Date(txtStartDate) >= new Date(txtEndDate)) {
            $('.endDate1').show();
            isValid = false;
            tbEndDate[i].style.border = "1px solid red";
            tbEndDate[i].style.background = "#FFCECE";
            var a = document.getElementById("error");
          
            //document.getElementById("error").style.visibility = "visible";
            //document.getElementsByClassName("endDate1").style.visibility = "visible";
        }

        else
        {
            //document.getElementById("error").style.visibility = "hidden";
           
            tbEndDate[i].style.border = "";
            tbEndDate[i].style.background = "";
            //$('.endDate1').hide();
        }  
    }


    $("#A2" + number).attr("disabled", "disabled");
    $("#A2" + number).removeClass("btn btn-danger");
    $("#A2" + number).addClass("greybutton");


    $("#A2" + number).prop('disabled', true);
    $('.check-' + number).each(function () {
        if ($(this).is(":checked")) {
            $(this).prop('checked', false);
        }
    });
    $('.selectall-' + number).prop('checked', false);

    $('.editAG' + number + ' option:selected').each(function () {
        if ($.trim($(this).val()) == 'Agency Name') {
            isValid = false;
            $('.editAG' + number + ' option:selected').css({
                "border": "1px solid red",
                "background": "#FFCECE"
            });
            $('.requiredel').show();
        } else {
            $('.editAG' + number + ' option:selected').css({
                "border": "",
                "background": ""
            });
            //$('.requiredel').hide();
        }


    });
    $('input[type="text"].editAID-' + number).each(function () {
        if ($.trim($(this).val()) == '') {
            isValid = false;
            $(this).css({
                "border": "1px solid red",
                "background": "#FFCECE"
            });
            if ($('.requiredel').hide())
                $('.requiredel1').show();

        } else {
            $(this).css({
                "border": "",
                "background": ""
            });
            //$('.requiredel1').hide();
        }
    });
    if (isValid == false) {
        e.preventDefault();
    }
    else {
        var GID = "";
        var AG = "";
        var AID = "";
        var complete = "";
        var start = "";
        var end = "";
        var url = "";

        $('.hiddenAG' + number).each(function () {
            GID += $(this).attr("value") + ",";

        });


        $('.editAG' + number + ' option:selected').each(function () {
            var str = $(this).val();
            var str1 = str.replace(/,/g, "");
            var str2 = str1.replace(/'/g, "");
            AG += str2 + ",";
        });

        $('.editstart-' + number).each(function () {
            //start.push($(this).val());
            if ($(this).val() == "") {
                start += empty1 + ",";

            }
            else {
                start += $(this).val() + ",";

            }
        });

        $('.editend-' + number).each(function () {
            if ($(this).val() == "") {
                end += empty1 + ",";
            }
            else {
                end += $(this).val() + ",";
            }
        });

        $('.editurl-' + number).each(function () {
            if ($(this).val() == "") {
                url += empty1 + ",";
            }
            else {
                var str = $(this).val();
                var str1 = str.replace(/,/g, "");
                var str2 = str1.replace(/'/g, "");
                url += str2 + ",";
            }
        });

        $('.editAID-' + number).each(function () {
            var str = $(this).val();
            var str1 = str.replace(/,/g, "");
            var str2 = str1.replace(/'/g, "");
            AID += str2 + ","

        });

        $('.completeEdit' + number).each(function () {
            if ($(this).is(':checked')) {
                complete += "True,"
            }
            else {
                complete += "False,"
            }
        });
        //var GID1 = GID;
        //var AG1 = AG;
        //var AID1 = AID;
        //var complete1 = complete;
        //var start1 = start;
        //var end1 = end;
        //var url1 = url;
        var r = confirm("Are you sure you want to update data ?");

        if (r == true) {
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                url: 'FundingNetwork.aspx/UpdateSource',
                data: "{'GID':'" + GID + "', 'Agency':'" + AG + "', 'AgencyID':'" + AID + "' , 'complete':'" + complete + "' , 'start':'" + start + "' , 'end':'" + end + "' , 'url':'" + url + "'}",
                async: false,
                success: function (response) {
                    $('#myModaledit' + number).modal("hide");
                    $('#h4Header').html("").html("Update");
                    $('#pBody').html("").html("Data Updated Successfully!");
                    $('#myModalDelete').modal("show");

                    //var innerValAG = "";
                    //var innerValAID = "";

                    //$('.editAG-' + number).each(function () {
                    //    innerValAG += $(this).val() + "</br>";
                    //    $("#AG-" + number).html(innerValAG);
                    //});

                    //$('.editAID-' + number).each(function () {
                    //    innerValAID += $(this).val() + "</br>";
                    //    $("#AID-" + number).html(innerValAID);

                    //});
                    //$('#myModaledit' + number).modal('toggle');
                    location.reload(true);
                },
                error: function ()
                { alert('There Is Some Error. Correct Input Data OR Contact For Support'); }
            });
        }
        else {

        }
    }

}



/// send data from popup to table
function addDataToTable(number, e) {
    //var startdate = $('.datepicke').val();
    //var enddate = $('.datepicke1').val();

    var isValid = true;
    var empty = "N/A";
    var tbStartDate1 = document.getElementsByName("startDate1" + number);
    var tbEndDate1 = document.getElementsByName("endDate1" + number);
    //alert(tbStartDate1.length);
    for (var i = 0; i < tbStartDate1.length; i++) {
        var txtStartDate1 = tbStartDate1[i].value;
        var txtEndDate1 = tbEndDate1[i].value;
        if (new Date(txtStartDate1) >= new Date(txtEndDate1)) {
            $('.endDate').show();
            isValid = false;
            tbEndDate1[i].style.border = "1px solid red";
            tbEndDate1[i].style.background = "#FFCECE";

        }

        else {
            //isValid = true;
            $('.endDate').hide();
            tbEndDate1[i].style.border = "";
            tbEndDate1[i].style.background = "";



        }

    }

    

    $('.clsAG' + number + ' option:selected').each(function () {
        if ($.trim($(this).val()) == "Agency Name") {
            isValid = false;
            $('.clsAG' + number).css({
                "border": "1px solid red",
                "background": "#FFCECE"
            });
            $('.require').show();
        } else {
            $('.clsAG' + number).css({
                "border": "",
                "background": ""
            });
            $('.require').hide();
        }
    });

    $('input[type="text"].clsAID-' + number).each(function () {
        if ($.trim($(this).val()) == '') {
            isValid = false;
            $(this).css({
                "border": "1px solid red",
                "background": "#FFCECE"
            });
            $('.require').show();
        } else {
            $(this).css({
                "border": "",
                "background": ""
            });
            $('.require').hide();
        }
    });
    if (isValid == false) {
        e.preventDefault();
    }
    else {
        $('.clscomplete-' + number).each(function () {

            if ($(this).is(':checked')) {
                complete += "True,"
            }
            else {
                complete += "False,"
            }
        });
        var innerValAG = "";
        var innerValAID = "";
        agency = "";
        $('.clsAG' + number + ' option:selected').each(function () {
            //var temp = $(this).text().replace(/,/g, "");
            agency += $(this).val() + ",";
        });
        award = "";
        $('.clsAID-' + number).each(function () {
            //var temp = $(this).text().replace(/,/g, "");
            award += $(this).val() + ",";
        });

        $('.clsAG' + number + ' option:selected').each(function () {
            var str = $(this).val();
            var str1 = str.replace(/,/g, "");
            var str2 = str1.replace(/'/g, "");
            innerValAG += "<br/>" + str2;
        });
        $("#AG-" + number).append(innerValAG);
        $('.clsAID-' + number).each(function () {
            var str = $(this).val();
            var str1 = str.replace(/,/g, "");
            var str2 = str1.replace(/'/g, "");
            innerValAID += "<br/>" + str2;
        });
        $("#AID-" + number).append(innerValAID);

        var AG = "";
        var AID = "";
        var com = "";
        var startdate = "";
        var enddate = "";
        var url = "";
        $('.clscomplete-' + number).each(function () {

            if ($(this).is(':checked')) {

                com += "True,"
            }
            else {


                com += "False,"
            }
        });

        $('.clsAG' + number + ' option:selected').each(function () {
            var str = $(this).val();
            var str1 = str.replace(/,/g, "");
            var str2 = str1.replace(/'/g, "");
            AG += str2 + ",";
        });
        award = "";
        $('.clsAID-' + number).each(function () {
            var str = $(this).val();
            var str1 = str.replace(/,/g, "");
            var str2 = str1.replace(/'/g, "");
            AID += str2 + ",";
        });

        $('#myModal' + number + ' input:text').each(function () {
            if ($(this).hasClass('datepicke')) {
                if ($(this).val() == "") {
                    startdate += empty + ",";
                }
                else {
                    startdate += $(this).val() + ",";
                }
            }
        });






        $('#myModal' + number + ' input:text').each(function () {
            if ($(this).hasClass('datepicke1')) {
                if ($(this).val() == "") {
                    enddate += empty + ",";
                }
                else {
                    enddate += $(this).val() + ",";
                }
            }

        });

        //$("#myModaledit" + number + " .datepicker").each(function () {
        //    if ($(this).val() == "") {
        //        startdate += null + ",";
        //    }
        //    else {
        //        startdate += $(this).val() + ",";
        //    }
        //});

        $('.url-' + number).each(function () {
            if ($(this).val() == "") {
                url += empty + ",";
            }
            else {
                var str = $(this).val();
                var str1 = str.replace(/,/g, "");
                var str2 = str1.replace(/'/g, "");
                url += str2 + ",";
            }
        });
        agencyAward.push({ "SourceId": number, "Agency": AG, "Award": AID, "IsChecked": com, "startdate": startdate, "enddate": enddate, "url": url });


        $("#Button1" + number).css({ "display": "" });
        $("#savebutton" + number).css({ "display": "" });
        $("#savebutton" + number).css({ "margin-left": "7px" });
        $("#Button1" + number).css({ "display": "none" });
        $("#myModal" + number).modal('hide');

    }


}


/// Remove required error on textboxes
function labelhide() {
    $('.require').hide();
}
var tempid = "";
var tempagency = "";
var tempaward = "";
var tempcom = "";
var tempstart = "";
var tempend = "";
var tempurl = "";

/// Send data from datatable to database
function addtodatabase1(number) {
    var count = 0
    $('#pleaseWaitDialog').show();
    $('#AG-' + number).each(function () {
        count++
    });
    if ($("#AG-" + number).text().trim() != "") {
        for (var i = 0; i < agencyAward.length; i++) {
            if (agencyAward[i].SourceId == number) {
                tempid = agencyAward[i].SourceId;
                tempagency += agencyAward[i].Agency;
                tempaward += agencyAward[i].Award;
                tempcom += agencyAward[i].IsChecked;
                tempstart += agencyAward[i].startdate;
                tempend += agencyAward[i].enddate;
                tempurl += agencyAward[i].url;
            }

        }

        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: 'FundingNetwork.aspx/SubmitSource',
            data: "{'source':'" + tempid + "', 'Agency':'" + tempagency + "', 'AgencyID':'" + tempaward + "', 'Complete':'" + tempcom + "', 'startdate':'" + tempstart + "', 'enddate':'" + tempend + "', 'url':'" + tempurl + "'}",
            async: false,
            success: function (response) {

                $("#cancel").remove();
                $("#ok").remove();
                $('#pBody1').html("").html("Data Saved Successfully!");
                $('#alert').modal({
                    backdrop: 'static',
                    keyboard: false
                })
                $("#alert").show();
                location.reload(true);
            },
            error: function () {
                //$('#pleaseWaitDialog').hide();
                $("#cancel1").remove();
                $("#ok1").remove();
                $('#pBody11').html("").html("There is some error !. Your data not saved.");
                $('#alert1').modal({
                    backdrop: 'static',
                    keyboard: false
                })
                $("#alert1").show();

            }
        });

    }
    else {
        $("#cancel1").remove();
        $("#ok1").remove();
        $('#pBody11').html("").html("Enter agency name and award id");
        $('#alert1').modal({
            backdrop: 'static',
            keyboard: false
        })
        $("#alert1").show();
    }
}



/// add rows on add popup
function addRow(number) {

    var tableID = "table-" + number;
    var classTBAG = "clsAG" + number;
    var classTBAID = "clsAID-" + number;
    var classcomplete = "clscomplete-" + number;
    var classurl = "url-" + number;
    var idTBAG = "latbox" + number;
    var idTBAID = "lngbox" + number;
    var dclass1 = "dated-" + number;
    var startDate = "startDate" + number;
    var endDate = "endDate" + number;
    var startDate1 = "startDate1" + number;
    var endDate1 = "endDate1" + number;
    $("#" + tableID + " tbody").append(
    "<tr class='mode1'>" +
    "<td><select class='" + classTBAG + "' style='width: 150px'><option selected='selected'>Agency Name</option><option value='National Aeronautics and Space Administration'>National Aeronautics and Space Administration</option><option value='National Science Foundation'>National Science Foundation</option>  <option value='Pennsylvania Department of Environmental Protection'>Pennsylvania Department of Environmental Protection</option>        <option value='U.S. Department Of Energy'>U.S. Department Of Energy</option><option value='U.S. Environmental Protection Agency'>U.S. Environmental Protection Agency</option><option value='U.S. Geological Survey'>U.S. Geological Survey</option></select></td>" +
    "<td><input style='width:110px' onkeydown = 'return (event.keyCode!=188);' class='" + classTBAID + "' type='text' Placeholder='Award ID'/></td>" +
    "<td><input type='text' style='width:110px' placeholder='MM/DD/YYYY' class='datepicke " + startDate + "' name='" + startDate1 + "' onkeydown='return ((((event.keyCode >= 48) && (event.keyCode <= 57)) || (event.keyCode == 191)) || (event.keyCode == 8));'/></td>" +
    "<td><input type='text' style='width:110px' placeholder='MM/DD/YYYY' class='datepicke1 " + endDate + "' name='" + endDate1 + "' onkeydown='return ((((event.keyCode >= 48) && (event.keyCode <= 57)) || (event.keyCode == 191)) || (event.keyCode == 8));'/></td>" +
    "<td><input type='text' style='width:110px' onkeydown = 'return (event.keyCode!=188);' placeholder='Grant URL' class='" + classurl + "' onkeydown='return (event.keyCode != 188);'/></td>" +
    "<td><input class='" + classcomplete + "' style='width:76px' type='checkbox' /> </td>" +
    "<td><a data-original-title='Tooltip on right' data-placement='left' data-toggle='tooltip' title='Delete row' href='javascript:;' onclick='deleteRow(this," + number + ")' class='icon-minus-sign deleteToolTip'></a></td>" +
    "</tr>");

    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
    var now1 = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);


    $('.datepicke').datepicker({
        onRender: function (date) {
            //return date.valueOf() < now.valueOf() ? 'disabled' : '';
        }
    }).on('select', function () { $(this).datepicker('hide'); });

    $('.datepicke1').datepicker({
        onRender: function (date) {
            //return date.valueOf() <= now1.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function () { $(this).datepicker('hide'); });
    // $(".datepicke1").datepicker().on('changeDate', function () { $(this).datepicker('hide'); });;




    // $(".datepicke").datepicker().on('select', function () { $(this).datepicker('hide'); });;


    $('.deleteToolTip').tooltip();

}


/// delete grants from databases
function deleteRowdatabase(number, e) {
    var count = 0;
    var r = confirm("Are you sure you want to delete data ?");
    if (r == true) {
        //$('#updatebutton' + number).prop('disabled', true);
        //$('.check-' + number).each(function () {
        //    if ($(this).is(':checked')) {
        //        GrantID += $(this).val() + ",";
        //    }
        //});

        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: 'FundingNetwork.aspx/Deleted',
            data: "{'GrantID':'" + number + "'}",
            async: false,
            success: function (response) {
                $('#myModaledit' + e).modal("hide");
                $('#h4Header').html("").html("Delete");
                $('#pBody').html("").html("Data Deleted Successfully!");
                $('#myModalDelete').modal("show");
                //var innerValAG = "";
                //var innerValAID = "";
                //var count = 0;
                //var count1 = 0;

                //$('.editAG-' + number).each(function () {
                //    innerValAG += $(this).val() + "</br>";
                //    $("#AG-" + number).html(innerValAG);
                //    count++
                //});

                //if (count == 0) {
                //    $("#AG-" + number).html('');

                //    $("#Button1" + number).css("display", "none");
                //    $("#updatebutton" + number).css("display", "none");
                //    //$("#A2" + number).css("display", "none");
                //    $('.selectall-' + number).hide();

                //    $("#savebutton" + number).prop('disabled', false);
                //    $("#savebutton" + number).text("Save");
                //}

                //$('.editAID-' + number).each(function () {
                //    innerValAID += $(this).val() + "</br>";
                //    $("#AID-" + number).html(innerValAID);
                //    count1++
                //});
                //if (count1 == 0) {
                //    $("#AID-" + number).html('');
                //}
                //$('#myModaledit' + number).modal('toggle');
                location.reload(true);
            },
            error: function ()
            { alert('There is some error'); }
        });
    }

    else {
        //if ($("#A" + number).attr("disabled") == "disabled") {
        //    e.preventDefault();  }
        $('.check-' + number).prop('checked', false);
    }
}



/// delete rows from add popup
function deleteRow(row, number) {
    var tableID = "table-" + number;
    var i = row.parentNode.parentNode.rowIndex;
    document.getElementById(tableID).deleteRow(i);
}
function deleteRowById(rowId, tableId) {
    document.getElementById(tableId).deleteRow(rowId);
}

