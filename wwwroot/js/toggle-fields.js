

$(document).ready(function () {
    //$('#VehicleInfo').show();
    //$('#Info').show();
    //$('#VehicleBranchInfo').hide();
    //$('#FuelInfo').show();
    //$('#FuelCost').show();
    //$('#DieselCost').show();
    //$('#PetrolCost').show();
    $('#BranchBlk').show();
    $('#DepartmentBlk').hide();



    $('#BDSelect').change(function () {
        var optionSelected = $('#BDSelect Option:Selected').text();
        if (optionSelected == "Branch"){
            $('#BranchBlk').show();
            $('#DepartmentBlk').hide();
            /*$('#HoursUsedByDifferentBranch').show();*/
        }
        else if (optionSelected == "Department") {
            $('#BranchBlk').hide();
            $('#DepartmentBlk').show();
         
        }
    })
})

//new tempusDominus.TempusDominus(document.getElementById("kt_td_picker_time_only"), {
//    display: {
//        viewMode: "clock",
//        components: {
//            decades: false,
//            year: false,
//            month: false,
//            date: false,
//            hours: true,
//            minutes: true,
//            seconds: false
//        }
//    }
//});