using Entities;

namespace AppConstants
{
    public class DatabaseRecipeCommands
    {
        public string SelectRecipeById(string database, int lineNumber, int recipeId)
            => $@"SELECT * FROM {database}.`l{lineNumber}_archive_recipe` as r 
                  WHERE r.id_recipe = {recipeId};";
        public string SelectRecipesByIds(string database, int lineNumber, IEnumerable<int> ids)
            => $@"SELECT * FROM {database}.`l{lineNumber}_archive_recipe` as r 
                  WHERE r.id_recipe IN ({string.Join(",", ids)});";
        public string SelectComponenentsFromRecipe(string database, int lineNumber, int recipeId)
            => $@"SELECT * FROM {database}.`l{lineNumber}_archive_component` AS l 
                  WHERE l.id_component IN (SELECT id_component FROM {database}.`l{lineNumber}_archive_recipe_structure` as r
                  WHERE r.id_recipe = {recipeId});";
        public string SelectRecipeIdsByName(string database, int lineNumber, string recipeName)
            => $@"SELECT id_recipe FROM l{lineNumber}_archive_recipe AS r 
                  WHERE r.name = '{recipeName}';";
        public string SelectAllRecipes(string database)
            => $@"SELECT * FROM {database}.`recipe`;";
        public string SelectAllArchiveRecipes(string database, int lineNumber)
            => $@"SELECT * FROM {database}.`l{lineNumber}_archive_recipe`;";

        /*                                                            {database}
         *  SELECT name, SUM(mass_auto) AS auto, SUM(mass_manual) AS manual, SUM(mass_correction) AS correction, SUM(mass_hold) AS hold FROM l1_batch_mats AS bm 
            INNER JOIN l1_archive_component AS c ON bm.id_component = c.old_id 
            WHERE bm.batch_id IN 
               (SELECT id_batch  FROM l1_batchs as b WHERE b.id_application IN 
               (SELECT id_recipe FROM l1_applications AS a WHERE a.start_time > '2021.05.21' AND a.end_time < '2021.05.22'))
				GROUP BY name;
         */

        /*
         *     SELECT
         *     lar.id_recipe,
         *     lar.name,
         *     SUM(la.volume_cur) AS volume,
         *     SUM(la.volume_fact) AS volume_fact
         *     FROM l2_applications la
         *
         *     INNER JOIN l2_layers lay
	     *     ON la.id_application = lay.app_id
         *
         *     INNER JOIN l2_archive_recipe lar
         *     ON lay.recipe_id = lar.id_recipe
         *
         *     WHERE lar.id_recipe <> 0  AND la.id_application IN (SELECT id_application FROM l2_applications) GROUP BY lar.name
         *     ORDER BY lar.name;
         *
         */
        public string SelectRecipesExpenditures(string database, int lineNumber, FilterData filter) //
            => $@"SELECT
                lar.id_recipe,
                  lar.name,
                  SUM(la.volume_cur) AS volume,
                  SUM(la.volume_fact) AS volume_fact

                FROM {database}.`l{lineNumber}_applications` la
  
                INNER JOIN {database}.`l{lineNumber}_layers` lay
	            ON la.id_application = lay.app_id

                INNER JOIN {database}.`l{lineNumber}_archive_recipe` lar
                ON lay.recipe_id = lar.id_recipe

                WHERE lar.id_recipe <> 0  AND la.id_application IN ({Constants.DatabaseCommands.Applications.SelectApplicationsByFilter(database, lineNumber, filter, values: "id_application")})GROUP BY lar.name
                ORDER BY lar.name;";
    }
}