


    $('#BlogContent').summernote({
        height: 300,                 // set editor height
        minHeight: null,             // set minimum height of editor
        maxHeight: null,             // set maximum height of editor
         focus: true
                         // set focus to editable area after initializing summernote


        }
    );



    





        function ValidateBlog() {

            var ErrorMessage = "";
            var fileExtension = ['jpeg', 'jpg', 'png' , 'JPG'];


            if ($("#BlogImage").val() != 0) {
                if ($.inArray($("#BlogImage").val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            ErrorMessage = ErrorMessage + "<div> <b> Sadece Jpeg jpg png formatında Resim girebilirsiniz </b> </div>";
                }
            }

            if ($("#BlogImage").val() == 0) {

            ErrorMessage = ErrorMessage + "<div> <b> Bir Resim Girmelisiniz. </b> </div>";

            }

            if ($("#BlogName").val() == false) {

            ErrorMessage = ErrorMessage + "<div> <b> Bir Blog Adı Girmelisiniz </b> </div>";

            }

            if ($("#BlogContent").val() == false) {

            ErrorMessage = ErrorMessage + "<div> <b> Bir Blog İçeriği Girmelisiniz. </b> </div>";

            }

            if ($("#BlogCategoryName").val() == false) {

            ErrorMessage = ErrorMessage + "<div> <b> Bir Blog Kategorisi Girmelisiniz. </b> </div>";

            }



            if (ErrorMessage == "") {

                return true;
            }

            else {

            $("#ErrorList").append(ErrorMessage);
                return false;
            }
        }

        //$("#kaydet").click(function (event) {

            //    debugger;

            //   // event.preventDefault();

            //    $("#ErrorList").html('');
            //    debugger;
            //    var validationResult = ValidatePortfolio();
            //    if (ValidatePortfolio() == true) {


            //        $("#myform").submit();

            //    }

            //    else {

            //        $("#ErrorList").removeClass("d-none");

            //    }

            //});


            $(document).on('submit', '#myform', function (event) {
                event.preventDefault();
                $("#ErrorList").html('');
                $("#ErrorList").addClass("d-none");
                $("#AddSuccess").addClass("d-none");




                var validationResult = ValidateBlog();
                console.log(validationResult);

                if (validationResult === true) {
                    console.log('ok');

                    console.log("Buraya geldim");


                    $.ajax({


                        url: '/Admin/AddBlog',
                        data: new FormData(this),
                        type: 'POST',
                        mimeType: "multipart/form-data",
                        cache: false,

                        contentType: false,
                        processData: false,




                        success: function (data, textStatus, xhr) {
                            $('#AddSuccess').removeClass('d-none');
                            console.log("success");
                            console.log(xhr.status);
                        },


                        error: function (request, status, error) {
                            console.log("error");
                        }


                    });

                    console.log("Buraya geldim2");





                }
                else {
                    $("html, body").animate({ scrollTop: $(window).scrollTop() - 135 }, "slow");

                    $("#ErrorList").removeClass("d-none");

                }

            });

