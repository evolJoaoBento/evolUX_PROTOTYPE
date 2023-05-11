// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(function () {

    var activeForm = "windows";
    $("#credentials-btn").on("click", () => {
        activeForm = "credentials";
        $("#credentials-form").show();
        $("#snake").show();
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
        $("#snake").hide();
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

/***************************************
Change/Replace class in document
    elementName: Element Name
    oldClassValue:	old class value
    newClassValue:	new class value
***************************************/
function changeClass(elementName, oldClassValue, newClassValue) {
    document.getElementById(elementName).classList.remove(oldClassValue);
    document.getElementById(elementName).classList.add(newClassValue);
}
/*************************************************************
Hide field with specific id
    elementName: name id of field to hide
**************************************************************/
function eShide(elementName) {
    try { document.getElementById(elementName).style.display = "none"; } catch (e) { }
}

/*************************************************************
Show field with specific id
    elementName: name id of field to show
**************************************************************/
function eSshow(elementName) {
    try { document.getElementById(elementName).style.display = ""; } catch (e) { }
}

/*************************************************************
Hide or Show div element with specific id
    opcional
        args[0+i]: objecto
        args[1+i]: 'show', 'hide'
**************************************************************/
function hideDivs() {
    var i, v, obj, args = hideDivs.arguments;
    for (i = 0; i < args.length - 1; i += 2) {
        if ((obj = document.getElementById(args[i])) != null) {
            v = args[i + 1];
            if (obj.style) {
                obj = obj.style;
                v = (v == 'show') ? 'visible' : (v = 'hide') ? 'hidden' : v;
            }
            obj.visibility = v;
        }
    }
}

/*************************************************************
Search an objecto in the page
    n: object to search
    d: where to search
**************************************************************/
function findObj(n, d) {
    var p, i, x;
    if (!d) d = document;
    if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
        d = parent.frames[n.substring(p + 1)].document;
        n = n.substring(0, p);
    }
    if (!(d[n]) && d.all)
        x = d.all[n];
    for (i = 0; !x && i < d.forms.length; i++)
        x = d.forms[i][n];
    for (i = 0; !x && d.layers && i < d.layers.length; i++)
        x = findObj(n, d.layers[i].document);
    return x;
}

/*******************************************************************************
Function that shows the detail by selecting the respective button
    startName: Element Start Name
********************************************************************************/
function showDetail(startName) {
    eSshow(startName);
    changeClass('arrow' + startName, 'bxs-chevron-down', 'bxs-chevron-up');
    document.getElementById('arrow' + startName).href = "javascript:hideDetail('" + startName + "');";
    changeClass('row|' + startName, 'evol-normal-row', 'evol-highlight-row');
}

/*******************************************************************************
Function that hides the detail by selecting the respective button
    startName: Element Start Name
********************************************************************************/
function hideDetail(startName) {
    eShide(startName);
    changeClass('arrow' + startName, 'bxs-chevron-up', 'bxs-chevron-down');
    document.getElementById('arrow' + startName).href = "javascript:showDetail('" + startName + "');";
    changeClass('row|' + startName, 'evol-highlight-row', 'evol-normal-row');
}
/*******************************************************************************
Function that shows the detail by selecting the respective button
    startName: Element Start Name
********************************************************************************/
function showDetails(startName) {
    eSshow(startName);
    changeClass('arrow' + startName, 'bxs-chevrons-down', 'bxs-chevrons-up');
    document.getElementById('arrow' + startName).href = "javascript:hideDetails('" + startName + "');";
}

/*******************************************************************************
Function that hides the detail by selecting the respective button
    startName: Element Start Name
********************************************************************************/
function hideDetails(startName) {
    eShide(startName);
    changeClass('arrow' + startName, 'bxs-chevrons-up', 'bxs-chevrons-down');
    document.getElementById('arrow' + startName).href = "javascript:showDetails('" + startName + "');";
}
/*******************************************************************************
Function that check all checkbox with specific elemnent name 
(optional: with specific elementId)
    elementName: name id of field to show
    elementId: start od value in checkbox
********************************************************************************/
function checkBoxAll(elementName, elementId) {
    myobj = document.getElementsByName(elementName);
    for (i = 0; (i < myobj.length); i++) {
        if (myobj[i].type == "checkbox"
            && (elementId == "" || myobj[i].value.substring(0, elementId.length) == elementId)) {
            if (!myobj[i].checked) {
                myobj[i].checked = true;
            }
        }
    }
}
/*******************************************************************************
Function that uncheck all checkbox with specific elemnent name 
(optional: with specific elementId)
    elementName: name id of field to show
    elementId: start od value in checkbox
********************************************************************************/
function uncheckBoxAll(elementName, elementId) {
    myobj = document.getElementsByName(elementName);
    for (i = 0; (i < myobj.length); i++) {
        if (myobj[i].type == "checkbox"
            && (elementId == "" || myobj[i].value.substring(0, elementId.length) == elementId)) {
            if (myobj[i].checked) {
                myobj[i].checked = false;
            }
        }
    }
}

function filterTableRows(form, table, elements) {
    form.addEventListener('submit', function (event) {
        event.preventDefault(); // prevent form from submitting

        const rows = table.getElementsByTagName('tr'); // get all table rows
        for (let i = 0; i < rows.length; i++) {
            let showRow = true;
            for (let j = 0; j < elements.length; j++) {
                const element = form.elements[elements[j]]; // get form element by name
                const cell = rows[i].children[elements[j]]; // get table cell in corresponding column
                if (element !== null && cell !== null && element !== undefined && cell !== undefined && element.value !== '')
                if ( cell.value !== element.value) {
                    showRow = false; // don't show row if it doesn't match
                    break;
                }
            }
            rows[i].style.display = showRow ? '' : 'none'; // show/hide row based on matching
        }
    });
}

function filterCheckBoxTableRows(form, table, elements) {
    form.addEventListener('submit', function (event) {
        event.preventDefault(); // prevent form from submitting
        const rows = table.getElementsByTagName('tr'); // get all table rows
        for (let i = 0; i < rows.length; i++) {
            let showRow = true;
            for (let j = 0; j < elements.length; j++) {
                const element = document.getElementsByName(elements[j]); // get form element by name
                const cell = rows[i].children[elements[j]]; // get table cell in corresponding column
                let value = '';
                for (let k = 0; k < element.length; k++) {
                    if (element[k].checked) {
                        value = value + element[k].value;
                    }
                }
                if (element !== null && cell !== null && element !== undefined && cell !== undefined && value !== '')
                    if (cell.value !== value) {
                        showRow = false; // don't show row if it doesn't match
                        break;
                    }
            }
            rows[i].style.display = showRow ? '' : 'none'; // show/hide row based on matching
        }
    });
}
const boxes = document.querySelectorAll('.alt-toggle-box');

boxes.forEach(box => {
    box.addEventListener('click', function () {

        boxes.forEach(cb => {
            if (cb !== this && cb.name == box.name) {
                cb.checked = false;
                cb.parentElement.classList.remove('checkbox-selected');
            } else if (cb == this && cb.checked) {
                cb.parentElement.classList.add('checkbox-selected');
            } else if (cb == this && !cb.checked) {
                cb.parentElement.classList.remove('checkbox-selected');
            }
        });
    });
});

const printerOpt = document.getElementById('print-options');
if (printerOpt != null) {
    printerOpt.addEventListener('click', function () {

        if (printerOpt.parentElement.classList.contains('print-filter-open')) {
            printerOpt.parentElement.classList.remove('print-filter-open');
        } else {
            printerOpt.parentElement.classList.add('print-filter-open');
        }
    });
}

var coll = document.getElementsByClassName("collapsible");
var i;

for (i = 0; i < coll.length; i++) {
    coll[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var content = this.nextElementSibling.querySelector('td').querySelector('div');
        if (content.style.height) {
            content.style.height = null;
            content.style.opacity = "0";
        } else {
            content.style.height = content.scrollHeight + 20 + "px";
            content.style.opacity = "100%";
        } 
    });
}
var scrollbar = document.querySelector('.scrollbar');
scrollbar.addEventListener('scroll', function () {
    var thumb = scrollbar.querySelector('::after');
    var percentage = scrollbar.scrollTop / (scrollbar.scrollHeight - scrollbar.clientHeight) * 100;
    thumb.style.top = percentage + '%';
});
