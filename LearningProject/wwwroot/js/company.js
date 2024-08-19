var dataTable;
$(document).ready(function () {
    loadDataTable();
});



function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: "/admin/company/getall" },
        "columns": [
            { data: 'companyName',"width":"20%" },
            { data: 'streetAddress', "width": "20%" },
            { data: 'city', "width": "10%" },
            { data: 'state', "width": "10%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'postalCode', "width": "10%" },
           
            {
                data: 'companyId',
                "render": function (data) {
                    return `<div class='w-75 btn-group' role='group'>
                    <a href='/admin/company/upsert?id=${data}' class='btn btn-primary mx-2 my-3>
                       <i class='bi bi-pencil-square'>Edit</i>
                    </a>

                     <a onClick=Delete('/admin/company/delete?id=${data}') class='btn btn-success mx-2 my-3>
                       <i class='bi bi-trash-fill'>Delete</i>
                    </a>

                    
                    
                    
                    
                    </div>`
                },
                "width": "25%"
            },
                        

        ]
    })
}


function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);

                }
            })
        }
    });
}