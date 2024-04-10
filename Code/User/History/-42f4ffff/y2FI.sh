#!/bin/bash

# Get the current active layout
current_layout=$(setxkbmap -query | awk '/layout/{print $2}')

# Toggle between English and Bulgarian Phonetic layouts
if [ "$current_layout" = "us" ]; then
    setxkbmap -layout bg -variant phonetic
else
    setxkbmap -layout us
fi
