4. Config file

4.1 Basic Configuration
Here is a basic config sample with all REQUIRED fields:

{
  "HAP": {
    "lidar_net_info" : {
      "cmd_data_port"  : 56000,
      "push_msg_port"  : 0,
      "point_data_port": 57000,
      "imu_data_port"  : 58000,
      "log_data_port"  : 59000
    },
    "host_net_info" : [
      {
        "lidar_ip"       : ["192.168.1.10","192.168.1.11","192.168.1.12", "192.168.1.13"],
        "host_ip"        : "192.168.1.5",
        "cmd_data_port"  : 56000,
        "push_msg_port"  : 0,
        "point_data_port": 57000,
        "imu_data_port"  : 58000,
        "log_data_port"  : 59000
      }
    ]
  }
}

Description for REQUIRED fields
"HAP": Lidar type, meaning the following configuration is for HAP lidar type; Another option is "MID360", for configuration of MID-360 lidar type.
"lidar_net_info": set the ports in the lidar.
"cmd_data_port": port for sending / receiving control command.
"push_msg_port": port for sending push message.
"point_data_port": port for sending point cloud data.
"imu_data_port": port for sending imu data.
"log_data_port": port for sending firmware log data.
"host_net_info": set the configuration of the host machines, and the value is a list, meaning that you can configure several hosts.
"lidar_ip": this is a list, indicating all ips of the lidars intended to connect to this host.
"host_ip": the ip of the host you're configuring.
"cmd_data_port": port for sending / receiving control command.
"push_msg_port" port for receiving push message.
"point_data_port": port for receiving point cloud data.
"imu_data_port": port for receiving imu data.
"log_data_port": port for receiving firmware log data.

4.2 Full Configuration
Here is a full sample including multi-lidar types configurations and some OPTIONAL fields:

{
  "master_sdk" : true,
  "lidar_log_enable"        : true,
  "lidar_log_cache_size_MB" : 500,
  "lidar_log_path"          : "./",

  "HAP": {
    "lidar_net_info" : {
      "cmd_data_port"  : 56000,
      "push_msg_port"  : 0,
      "point_data_port": 57000,
      "imu_data_port"  : 58000,
      "log_data_port"  : 59000
    },
    "host_net_info" : [
      {
        "lidar_ip"       : ["192.168.1.10","192.168.1.11","192.168.1.12", "192.168.1.13"],
        "host_ip"        : "192.168.1.5",
        "multicast_ip"   : "224.1.1.5",
        "cmd_data_port"  : 56000,
        "push_msg_port"  : 0,
        "point_data_port": 57000,
        "imu_data_port"  : 58000,
        "log_data_port"  : 59000
      }
    ]
  },
  "MID360": {
    "lidar_net_info" : {
      "cmd_data_port"  : 56100,
      "push_msg_port"  : 56200,
      "point_data_port": 56300,
      "imu_data_port"  : 56400,
      "log_data_port"  : 56500
    },
    "host_net_info" : [
      {
        "lidar_ip"       : ["192.168.1.3"],
        "host_ip"        : "192.168.1.5",
        "multicast_ip"   : "224.1.1.5",
        "cmd_data_port"  : 56101,
        "push_msg_port"  : 56201,
        "point_data_port": 56301,
        "imu_data_port"  : 56401,
        "log_data_port"  : 56501
      }
    ]
  }
}

Description for OPTIONAL fields
"master_sdk": used in multi-casting scenario.
'true' stands for master SDK and 'false' stands for slave SDK;
'master SDK' can send control command to and receive data from the lidars, while 'slave SDK' can only receive point cloud data from the lidars.
NOTICE: ONLY ONE SDK (host) can be set as 'master SDK'. Others should be set as 'slave SDK'.
"lidar_log_enable": 'true' or 'false' represents whether to enable the firmware log.
"lidar_log_cache_size_MB": set the storage size for firmware log, unit: MB.
"lidar_log_path": set the path to store the firmware log data.
"multicast_ip": this field is in the parent key "host_net_info", representing the multi-casting IP.