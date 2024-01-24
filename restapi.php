<?php

include("connection.php");
$db = new dbObj();
$connection =  $db->getConnstring();
$request_method=$_SERVER["REQUEST_METHOD"];


switch ($request_method) 
{
    case 'GET':  

        if(!empty($_GET["rajtszam"]))
        {
            $id=intval($_GET["rajtszam"]); 
            get_driversid($id);
        }
        else
        {
            get_drivers();
        }
        break;
    case 'POST':

        insert_driver(); 
        break;

    case 'PUT':

        $id=intval($_GET["rajtszam"]);
        update_driver($id);
        break;
    case 'DELETE':

        $id=intval($_GET["rajtszam"]);
        delete_driver($id);
        break;
    default:

        header("HTTP/1.1 405 Method Not Allowed");
        break;
}

function get_drivers()
{
  global $connection;
  $query="SELECT * FROM drivers";
  $response=array();
  $result=mysqli_query($connection, $query);
  while($row=mysqli_fetch_array($result))
  {
    $response[]=$row;
  }
  header('Content-Type: application/json'); 
  echo json_encode($response); 
}


function get_driversid($id=0)
{
  global $connection;
  $id=isset($_GET["rajtszam"]) ? intval($_GET["rajtszam"]) : 0;
  $query="SELECT * FROM drivers";
  if($id != 0)
  {
    $query.=" WHERE rajtszam=".$id; 
  }
  $response=array();
  $result=mysqli_query($connection, $query);
  while($row=mysqli_fetch_array($result))
  {
    $response[]=$row;
  }
  header('Content-Type: application/json'); 
  echo json_encode($response); 
}

function insert_driver()
 {
  global $connection;
   
    $data = json_decode(file_get_contents('php://input'), true); 
    $rajtszam=$data["rajtszam"]; 
    $nev=$data["nev"];
    $csapat=$data["csapat"];
    $szuletesiev=$data["szuletesiev"];
    echo $query="INSERT INTO drivers SET rajtszam='".$rajtszam."', nev='".$nev."', csapat='".$csapat."', szuletesiev='".$szuletesiev."'";
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

function delete_driver($id)
{
   global $connection;
   $query="DELETE FROM drivers WHERE rajtszam=".$id;
   if(mysqli_query($connection, $query))
   {
     $response=array(
      'status' => 1,
      'status_message' =>'Driver Deleted Successfully.'
      );
   }
   else
   {
      $response=array(
         'status' => 0,
         'status_message' =>'Driver Deletion Failed.'
      );
   }
   header('Content-Type: application/json');
   echo json_encode($response);
}

function update_driver($id)
 {
   global $connection;
   $data = json_decode(file_get_contents("php://input"),true);
   $rajtszam=$data["rajtszam"]; 
    $nev=$data["nev"];
    $csapat=$data["csapat"];
    $szuletesiev=$data["szuletesiev"];
   $query="UPDATE drivers SET nev='".$nev."', csapat='".$csapat."', szuletesiev='".$szuletesiev."' WHERE rajtszam=".$rajtszam;
   if(mysqli_query($connection, $query))
   {
      $response=array(
         'status' => 1,
         'status_message' =>'Driver Updated Successfully.'
      );
    }
    else
    {
        $response=array(
            'status' => 0,
           'status_message' =>'Driver Updation Failed.'
        );
    }
    header('Content-Type: application/json');
    echo json_encode($response);
}
?>

