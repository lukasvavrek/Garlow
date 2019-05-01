#!/bin/bash

python people_counter.py \
  --prototxt mobilenet_ssd/MobileNetSSD_deploy.prototxt \
  --model mobilenet_ssd/MobileNetSSD_deploy.caffemodel \
  --input rtsp://camera.openlab.kpi.fei.tuke.sk/proxyStream-8 \
  --publicid 2790f8df-7508-4d7d-80bd-a16943f361ca \
  --secretkey 52KNGLBH2XGZG1YIKFZ8IWDDMNKIOLXI

