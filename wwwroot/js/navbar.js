
var navbarToggle = false;

function toggle(){


    switch(navbarToggle){
        case false:
            document.getElementById('navbarMiniWrap').style.display = 'inline';
            document.getElementById('navbarMiniWrap').style.backgroundColor = '#30303b'
            navbarToggle = true;
            break;
        case true:
            document.getElementById('navbarMiniWrap').style.display = 'none'
            document.getElementById('navbarMiniWrap').style.backgroundColor = '#30303b'
            navbarToggle = false;
            break;
    }

    /*
    if (navbarToggle == false){
        document.getElementById('navbarMiniWrap').style = 'display: inline;';
        navbarToggle = true;
    }
    else if (navbarToggle == true){
        document.getElementById('navbarMiniWrap').style = 'display: none;';
        navbarToggle = false;
    }
    */

}