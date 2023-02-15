#!/bin/bash
REPOSITORIES=()

if [ "$1" = "-p" ]
  then
    echo ${REPOSITORIES[@]} | sed -E -e 's/[[:blank:]]+/\n/g' | xargs -I {} -n 1 -P 0 sh -c 'printf "========================================================\nUpdating repository: {}\n========================================================\n"; git -C {} checkout develop; git -C {} pull; git -C {} checkout master; git -C {} pull;git -C {} checkout develop;'
  else
    for REPOSITORY in ${REPOSITORIES[*]}
    do
      echo ========================================================
      echo Updating repository: $REPOSITORY
      echo ========================================================
      cd $REPOSITORY
      git checkout develop
      git pull
      git checkout master
      git pull
      git checkout develop
      cd ..
    done
fi