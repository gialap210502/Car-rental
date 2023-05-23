// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
            // $(function() {
            //     $(".modal-dialog").mousedown(function(e) {
            //         var offset = $(this).offset();
            //         var xPos = e.pageX - offset.left;
            //         var yPos = e.pageY - offset.top;
            //         $(document).mousemove(function(e) {
            //             var newX = e.pageX - xPos;
            //             var newY = e.pageY - yPos;
            //             $(".modal-dialog").css("left", newX + "px");
            //             $(".modal-dialog").css("top", newY + "px");
            //         });
            //     });
            //     $(document).mouseup(function() {
            //         $(document).unbind("mousemove");
            //     });
            // });
        }
    })
}

