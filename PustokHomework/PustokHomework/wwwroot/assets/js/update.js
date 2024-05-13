//$(document).ready(function () {

//    $("#profileForm").submit(function (e) {
//        e.preventDefault();


//        let url = $(this).attr("action");


//        fetch(url)
//            .then(res=> {
//                if (res.ok) {
//                    Swal.fire({
//                        position: "top-end",
//                        icon: "success",
//                        title: "Your work has been saved",
//                        showConfirmButton: false,
//                        timer: 1500

//                    }).then(() => {
//                        window.location.reload();
//                    })
//                }
//                else {
//                    Swal.fire({
//                        position: "top-end",
//                        icon: "error",
//                        title: "Your work has been saved",
//                        showConfirmButton: false,
//                        timer: 1500

//                    })
//                }
//            })

//    })
//})



//$(document).ready(function (e) {
//    $("#profileForm").submit(function (e) {
//        e.preventDefault();

//        let url = $(this).attr("account/myAccount");

//        console.log(url)

//        fetch(url)
//            .then(res => {
//                if (res.ok) {
//                    Swal.fire({
//                        position: "top-end",
//                        icon: "success",
//                        title: "Your work has been saved",
//                        showConfirmButton: false,
//                        timer: 1500
//                    }).then(() => {
//                        window.location.reload();
//                    });
//                }
//                else {
//                    Swal.fire({
//                        position: "top-end",
//                        icon: "error",
//                        title: "Your work has not been saved",
//                        showConfirmButton: false,
//                        timer: 1500
//                    });
//                }
//            });
//    });
//});
