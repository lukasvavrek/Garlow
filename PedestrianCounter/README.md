# Pedestrian counter

Video stream processor that monitors pedestrians and log an info when they go in/out of the specified area.

## Run

```
python people_counter.py \
  --prototxt mobilenet_ssd/MobileNetSSD_deploy.prototxt \
  --model mobilenet_ssd/MobileNetSSD_deploy.caffemodel \
  --input ./videos/example_01.mp4 \
  --publicid 2790f8df-7508-4d7d-80bd-a16943f361ca \
  --secretkey 52KNGLBH2XGZG1YIKFZ8IWDDMNKIOLXI
```

