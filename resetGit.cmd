echo Achtung: Alle lokalen �nderungen werden zur�ckgesetzt. Druecke CTRL+C zum Abbrechen.
pause
git fetch --all
git reset --hard origin/master
