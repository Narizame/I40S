#!/bin/bash

eww open bar0
if [ $monitors -gt 1 ]; then
	for ((i = 1 ; i <= $monitors ; i++)); do
		eww open bar${i}
	done
fi
