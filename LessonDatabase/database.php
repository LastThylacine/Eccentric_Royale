<?php
    if (interface_exists('RedBeanPHP\Logger')) return;

    require 'RedbeanPHP/rb-mysql.php';

    R::setup( 'mysql:host=localhost;dbname=testbd', 'root', '' );

    if(R::testConnection() == false){
        echo 'Подключение к БД не удалось';
        exit;
    }
?>