<?php
    require '../database.php';
    require '../System/configConstants.php';

    $key = $_POST['key'];
    $userID = $_POST['userID'];

    if(!isset($userID) || !isset($key) || $key != KEY) {
        echo 'Data struct error';
        exit; 
    }

    $user = R::load('users', $userID);

    $selectedCardBeans = $user -> withCondition('cards_users.selected = ?', array(true)) -> sharedCards;
    
    $selectedIDs = json_encode(array_column($selectedCardBeans, 'id'));

    echo $selectedIDs;
?>