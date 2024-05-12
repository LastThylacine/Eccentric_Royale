<?php
    require '../database.php';

    $userID = $_POST['userID'];

    if(!isset($userID)){
        echo 'Data struct error';
        exit;
    }

    $user = R::load('users', $userID);
    $allCards = $user -> sharedCards;

    $avaliableCards = [];

    foreach($allCards as $card){
        $avaliableCards[] = $card -> export();
    }

    $avaliableCardsJson = json_encode($avaliableCards);

    $selectedCardBeans = $user -> withCondition('cards_users.selected = ?', array(true)) -> sharedCards;
    
    $selectedIDs = json_encode(array_column($selectedCardBeans, 'id'));

    echo '{"avaliableCards":'. $avaliableCardsJson .', "selectedIDs":'. $selectedIDs .'}';
?>