#!/bin/sh

VERSION=$1
if [ -z "$VERSION" ]; then
    echo "Must specify version."
    exit 1
fi

zip LiveSplit.LoadTimer_v${VERSION}.zip LiveSplit.LoadTimer.dll ../README.txt