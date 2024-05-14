<?php 
    require '../database.php';

    $userID = 1;//$_POST['userID'];

    $user = R::load('users', $userID);

    $rating = $user -> fetchAs('ratings') -> rating;

    echo 'ok|'.$rating -> win.'|'.$rating -> loss;
?>