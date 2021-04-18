var spravnaOdpovedIndex;

var spravnaOdpoved;

var a;
var b;
var c;
var d;

var randomizedList;

window.onload = function() {
    getQuestion();
};



function getRandomInt(max){
    return Math.floor(Math.random() * Math.floor(max));
}


function getQuestion(){

    var otazkaText = document.getElementById('questionText').innerText = 'loading...';
    var moznostiText = document.getElementsByClassName('moznosti');

    for (var i = 0; i < moznostiText.length; i++){
        moznostiText[i].innerHTML = 'loading...';
    }
    

    var kategorie = document.getElementById('categories');
    var difficulty = document.getElementById('difficulty');
    var scoreText = document.getElementById('scoreText');

    var categorySet;
    var difficultySet;
    switch(kategorie.value)
    {
        case 'all':
            categorySet = '';
            break;
        case 'it':
            categorySet = '&category=18';
            break;
        case 'games':
            categorySet = '&category=15';
            break;
        case 'anime':
            categorySet = '&category=31';
            break;
        case 'history':
            categorySet = '&category=23';
            break;
        case 'sport':
            categorySet = '&category=21';
            break;
    }
    switch(difficulty.value)
    {
        case 'random':
            difficultySet = '';
            break;
        case 'easy':
            difficultySet = '&difficulty=easy';
            break;
        case 'medium':
            difficultySet = '&difficulty=medium';
            break;
        case 'hard':
            difficultySet = '&difficulty=hard';
            break;

    }
    url = 'https://opentdb.com/api.php?amount=1&type=multiple' + categorySet + difficultySet;
    fetch(url)
    .then(res => res.json())
    .then(data => changeHtml(data))
}
function changeHtml(data){

    resetHtml();

    var difficulty = document.getElementById('difficulty');
    var difficultyText = document.getElementById('difficultyText');

    difficultyText.innerHTML = (difficulty.value);

    var questionText = document.getElementById('questionText');

    questionText.innerHTML = data["results"][0]["question"];

    var listOfQuestions = [];
    listOfQuestions.push(data["results"][0]["incorrect_answers"][0],
                         data["results"][0]["incorrect_answers"][1],
                         data["results"][0]["incorrect_answers"][2],
                         data["results"][0]["correct_answer"]);

    //console.log(listOfQuestions);
    randomizedList = listOfQuestions.sort(() => Math.random() - 0.5)
    //console.log(randomizedList);

    
    document.getElementById('Aansw').innerHTML = `a) ${randomizedList[0]}`;
    document.getElementById('Bansw').innerHTML = `b) ${randomizedList[1]}`;
    document.getElementById('Cansw').innerHTML = `c) ${randomizedList[2]}`;
    document.getElementById('Dansw').innerHTML = `d) ${randomizedList[3]}`;

    a = randomizedList[0];
    b = randomizedList[1];
    c = randomizedList[2];
    d = randomizedList[3];


    spravnaOdpoved = (data["results"][0]["correct_answer"]);

    spravnaOdpovedIndex = randomizedList.indexOf(data["results"][0]["correct_answer"]);
    console.log(randomizedList[spravnaOdpovedIndex]);
    console.log(spravnaOdpovedIndex);

}

function resetHtml(){

    enableButtons();

    document.getElementById('Abut').style.backgroundColor = '#30303b';
    document.getElementById('Abut').style.boxShadow = '';
    document.getElementById('Abut').style.animation = 'none';
    document.getElementById('Bbut').style.backgroundColor = '#30303b';
    document.getElementById('Bbut').style.boxShadow = '';
    document.getElementById('Bbut').style.animation = 'none';
    document.getElementById('Cbut').style.backgroundColor = '#30303b';
    document.getElementById('Cbut').style.boxShadow = '';
    document.getElementById('Cbut').style.animation = 'none';
    document.getElementById('Dbut').style.backgroundColor = '#30303b';
    document.getElementById('Dbut').style.boxShadow = '';
    document.getElementById('Dbut').style.animation = 'none';
}

function checkQuestion(letter){
    switch(letter)
    {
        case 'a':
            if (randomizedList.indexOf(String(Aansw.innerHTML).substring(3)) == spravnaOdpovedIndex) spravne('Abut');
            else spatne('Abut');
            break;
        case 'b':
            if (randomizedList.indexOf(String(Bansw.innerHTML).substring(3)) == spravnaOdpovedIndex) spravne('Bbut');
            else spatne('Bbut');
            break;
        case 'c':
            if (randomizedList.indexOf(String(Cansw.innerHTML).substring(3)) == spravnaOdpovedIndex) spravne('Cbut');
            else spatne('Cbut');
            break;
        case 'd':
            if (randomizedList.indexOf(String(Dansw.innerHTML).substring(3)) == spravnaOdpovedIndex) spravne('Dbut');
            else spatne('Dbut');
            break;
    }
}






function spravne(tlacitko){

    var tlacitko = document.getElementById(tlacitko);

    tlacitko.style.boxShadow = '0 0px 10px #39ff14'
    tlacitko.style.backgroundColor = '#39ff14'
    //tlacitko.style.animation = 'shake 0.2s';

    var difficulty = document.getElementById('difficulty');


    var jmenoUzivatele = document.getElementById('userName');
    var questionText = document.getElementById('questionText');



    if(jmenoUzivatele.innerText != 'Guest'){
        var scoreKpricteni;
    
        switch(difficulty.value){
            case 'random':
                scoreKpricteni = 750
                break;
            case 'easy':
                scoreKpricteni = 250
                break;
            case 'medium':
                scoreKpricteni = 500
                break;
            case 'hard':
                scoreKpricteni = 1000
                break;

        }



        $.post('/Trivia/trivia', { jmeno: String(jmenoUzivatele.innerText),
                            score: String(scoreKpricteni) });


        var scoreText = document.getElementById('scoreText');
        scoreText.innerHTML = String(parseInt(scoreText.innerHTML) + scoreKpricteni)
    
        questionText.innerHTML = `SPRAVNE + ${scoreKpricteni} bodu!!!`



    }
    else{
        questionText.innerHTML = `SPRAVNE!`
    }


    setTimeout(getQuestion, 2000);

    //getQuestion();

}

function spatne(tlacitko){

    disableButtons();

    var tlacitko = document.getElementById(tlacitko);

    tlacitko.style.boxShadow = '0 0px 10px #ff073a'
    tlacitko.style.backgroundColor = '#ff073a'
    tlacitko.style.animation = 'shake 0.2s';


    var questionText = document.getElementById('questionText');
    questionText.innerHTML = `SPATNE! spravna odpoved byla '${spravnaOdpoved}'`;
    setTimeout(getQuestion, 2000);
}

function disableButtons(){
    document.getElementById('Abut').disabled = true;
    document.getElementById('Bbut').disabled = true;
    document.getElementById('Cbut').disabled = true;
    document.getElementById('Dbut').disabled = true;
}
function enableButtons(){
    document.getElementById('Abut').disabled = false;
    document.getElementById('Bbut').disabled = false;
    document.getElementById('Cbut').disabled = false;
    document.getElementById('Dbut').disabled = false;
}