#!/bin/bash

xbindkeys \
  --batch "$(
    hyprctl devices -j |
      jq -r '.keyboards[] | .name' |
      while IFS= read -r keyboard; do
        printf '%s %s %s;' 'setxkbmap -device' "${keyboard}" '-layout next'
      done
  )"
