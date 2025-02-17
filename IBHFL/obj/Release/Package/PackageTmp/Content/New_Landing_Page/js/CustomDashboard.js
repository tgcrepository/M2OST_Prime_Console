







function Mouseclickcall() {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-bottom-full-width",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "200",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
        //"timeOut": 0,
        //"extendedTimeOut": 0

    }
    toastr["error"]("Right Click Disabled");
    toastr.options = null;
}

$(document).ready(function () {
  

    //$('.main_body').bind("cut copy paste", function (e) {
    //    toastr["error"]("Cut - Copy - Paste Disabled");
    //    e.preventDefault();
    //});

    $('.main_body').on("cut copy paste", function (e) {
        console.log(e)
        // Check if the target is not the input field with the ID "url"
        if (!$(e.target).is('#url,#Participants')) {
            toastr["error"]("Cut - Copy - Paste Disabled");
            e.preventDefault();
        }
    });



});
