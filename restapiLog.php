<?php

include("connectionLog.php");
$db = new dbObj();
$connection =  $db->getConnstring();
$request_method=$_SERVER["REQUEST_METHOD"];

switch ($request_method) 
{
    case 'GET':  

        get_user();
        break;
    case 'POST':

        insert_user(); 
        break;
    default:

        header("HTTP/1.1 405 Method Not Allowed");
        break;
}

function insert_user()
 {
  global $connection;
   
    $data = json_decode(file_get_contents('php://input'), true); 
    $userName=$data["userName"]; 
    $password=$data["password"];
    $admin=$data["admin"];
    echo $query="INSERT INTO userok SET userName='".$userName."', password='".$password."', admin='".$admin."'";
    if(mysqli_query($connection, $query))
    {
       $response=array(
             'status' => 1,
             'status_message' =>'Driver Added Successfully.'
              );
    }
    else
    {
       $response=array(
             'status' => 0,
             'status_message' =>'Drvier Addition Failed.'
             );
    }
    header('Content-Type: application/json');
    echo json_encode($response); 
}

function get_user()
{
    global $connection; 

    if(isset($_GET["userName"])) {
        $userName = $_GET["userName"];
        $stmt = $connection->prepare("SELECT * FROM userok WHERE userName = ?");
        $stmt->bind_param("s", $userName);
        $stmt->execute();
        $result = $stmt->get_result();
        
        $response = [];
        while($row = $result->fetch_assoc()) {
            $row["admin"] = $row["admin"] == 1 ? true : false;
            $response[] = $row;
        }
        
        header('Content-Type: application/json'); 
        echo json_encode($response); 
    }
}



?>