// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ConfirmDeletion(url) {
    Swal.fire({
        title: "Are you sure you want to delete?",
        icon: 'warning',
        text: "You will not be able to restore the content!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnconfirm: true,
        background: '#41454d'
    }).then((result) => {
        if (result.isConfirmed) {
            RemoveFromDB(url);
        }
    })
}

function RemoveFromDB(url) {
    $.ajax({
        type: 'DELETE',
        url: url,
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}