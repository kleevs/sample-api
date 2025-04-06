# GitFlow pour le développement d'une nouvelle feature
1. Création de la branche à partir de la branche develop (`git checkout develop & git checkout -b feature/<numéro du ticket>`). Le nom de la branche peut être de la forme :
  - feature/&lt;numéro du ticket&gt;
  - feature/&lt;numéro du ticket&gt;-&lt;description&gt;

  Le nom des branches doivent être en minuscule.

2. Mettre à jour la branche quotidiennement à partir de la branch develop (`git pull origin develop --rebase`)

3. Effectuez une PR vers la branche develop pour livrer la feature sur l'environnement de dev.