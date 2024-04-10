#!/bin/bash

# Get the current active layout
current_layout=$(xbindkeys -query | awk '/layout/{print $2}')

# Toggle between English and Bulgarian Phonetic layouts
if [ "$current_layout" = "us" ]; then
    xbindkeys -layout bg -variant phonetic
else
    xbindkeys -layout us
fi
