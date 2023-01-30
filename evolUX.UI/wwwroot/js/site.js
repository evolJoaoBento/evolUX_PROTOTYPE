// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(function () {

    var activeForm = "windows";
    $("#credentials-btn").on("click", () => {
        activeForm = "credentials";
        $("#credentials-form").show();
        $("#credentials-form").addClass("active");
        $("#windows-form").removeClass("active");
        $("#windows-form").hide();
    })

    $("#windows-btn").on("click", () => {
        activeForm = "windows"
        $("#windows-form").show();
        $("#windows-form").addClass("active");
        $("#credentials-form").removeClass("active");
        $("#credentials-form").hide();
    })
    $("#login-btn").on("click", () => {
        $("#" + activeForm + "-form").submit();
    });

    let subMenuArrow = document.querySelectorAll(".sidebar .nav-links li i.arrow");
    for (var i = 0; i < subMenuArrow.length; i++) {
        subMenuArrow[i].addEventListener("click", (e) => {
            let arrowParent = e.target.parentElement.parentElement;//selecting main parent of arrow
            arrowParent.classList.toggle("showMenu");
        });
    }

    let subSubMenuArrow = document.querySelectorAll(".sidebar .nav-links li .sub-menu li .sub-sub-menu-title i.arrow");
    for (var i = 0; i < subSubMenuArrow.length; i++) {
        subSubMenuArrow[i].addEventListener("click", (e) => {
            let arrowParent = e.target.parentElement.parentElement;//selecting main parent of arrow
            arrowParent.classList.toggle("showSubMenu");
        });
    }

    let sidebar = document.querySelector(".sidebar");
    
    let colapseIcons = document.querySelectorAll(".collapse-icon");
    colapseIcons.forEach(collapseIcon => {
        collapseIcon.addEventListener("click", () =>
        {
            if (sidebar.classList.contains("close")) {
                //let width = document.getElementById("sidebar").offsetWidth;
                //document.getElementById("sidebar").style.minWidth = width;
            } else {
                let width = document.getElementById("sidebar").offsetWidth;
                document.getElementById("sidebar").style.minWidth = width;

            }
            sidebar.classList.toggle("close");


        });
    });

});
