// The following snippet works around a problem where FloatingBehavior
// doesn't allow drops outside the "content area" of the page - where "content
// area" is a little unusual for our sample web pages due to their use of CSS
// for layout.
function setBodyHeightToContentHeight() {
    document.body.style.height = Math.max(document.documentElement.scrollHeight, document.body.scrollHeight) + "px";
}
//setBodyHeightToContentHeight();
//$addHandler(window, "resize", setBodyHeightToContentHeight);
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function clearlistcontrol(lc) {
    for (var i = lc.options.length - 1; i >= 0; i--) {
        lc.options[i] = null;
    }
    lc.selectedIndex = -1;

}

function SelectListValue(l, value) {
    var ind = 0;
    for (var i = l.options.length - 1; i >= 0; i--) {
        if (l.options[i].value == value) {
            ind = l.options[i].index;
            break;
        }
    }
    l.selectedIndex = ind;
}

function AddItem(l, Text, Value) {
    // Create an Option object        

    var opt = document.createElement("option");

    // Add an Option object to Drop Down/List Box
    l.options.add(opt);

    // Assign text and value to Option object
    opt.text = Text;
    opt.value = Value;
}

function reloadListControl(lc, List) {
    //clear the listcontrol
    clearlistcontrol(lc);
    for (var x in List) {
        //add each email item to the listbox
        AddItem(lc, List[x].text, List[x].value);
    }
}

////to use set onKeyPress event in <input type="text" />
////onKeyPress="return numbersonly(this, event)"
//function numbersonly(myfield, e, dec) {
//    // copyright 1999 Idocs, Inc. http://www.idocs.com
//    // Distribute this script freely but keep this notice in place
//    var key;
//    var keychar;

//    if (window.event)
//       key = window.event.keyCode;
//    else if (e)
//       key = e.which;
//    else
//       return true;
//    keychar = String.fromCharCode(key);

//    // control keys
//    if ((key==null) || (key==0) || (key==8) || 
//        (key==9) || (key==13) || (key==27) )
//       return true;

//    // numbers
//    else if ((("0123456789").indexOf(keychar) > -1))
//       return true;

//    // decimal point jump
//    else if (dec && (keychar == "."))
//       {
//       myfield.form.elements[dec].focus();
//       return false;
//       }
//    else
//       return false;
//}

function numbersonly(e) {
    var key;
    var keychar;

    if (window.event)
        key = window.event.keyCode;
    else if (e)
        key = e.which;
    else
        return true;
    keychar = String.fromCharCode(key);

    // control keys
    if ((key == null) || (key == 0) || (key == 8) ||
        (key == 9) || (key == 13) || (key == 27))
        return true;

    // numbers
    else if ((("0123456789").indexOf(keychar) > -1))
        return true;

    // decimal point jump
   // else if (dec && (keychar == ".")) {
   //     myfield.form.elements[dec].focus();
    //    return false;
    //}
    else
        return false;
}

