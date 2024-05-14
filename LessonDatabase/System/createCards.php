<?php
    require '../database.php';

    $cardsNames = array('Archer', 'Archer_Sunflower', 'Golem', 'Golem_Sting', 'Golem_Silver', 'Golem_Transparent', 'Golem_Brilliant', 'Warrior', 'Warrior_Phantom');

    foreach($cardsNames as $name){
        $card = R::dispense('cards');
        $card -> name = $name;
        R::store($card); 
    }
?>