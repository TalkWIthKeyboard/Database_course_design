var loginButton = document.querySelector('.login-button');
var signupButton = document.querySelector('.signup-button');
var activeElements = [document.querySelector('.buttons'),
                    document.querySelector('.log-link'),
                    document.querySelector('.sign-link'),
                    document.querySelector('.login-underline'),
                    document.querySelector('.signup-underline'),
                    document.querySelector('.login-form'),
                    document.querySelector('.signup-form')];

loginButton.onclick = function (e) {
    e.preventDefault();
    for (var i = 0; i < activeElements.length; i++) {
        activeElements[i].classList.remove('signup-button-active');
        activeElements[i].classList.add('login-button-active');
    }
}

signupButton.onclick = function (e) {
    e.preventDefault();
    for (var i = 0; i < activeElements.length; i++) {
        activeElements[i].classList.remove('login-button-active');
        activeElements[i].classList.add('signup-button-active');
    }
}