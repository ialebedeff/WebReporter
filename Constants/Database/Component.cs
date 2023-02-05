using Entities;

namespace AppConstants
{
    public class DatabaseComponentCommands
    {
        //public string SelectComponentExpenditures(string database, int lineNumber, FilterData filterData)
        //   => $@"SELECT name, SUM(mass_auto) AS auto, SUM(mass_manual) AS manual, SUM(mass_correction) AS correction FROM l{lineNumber}_batch_mats AS bm 
        //          INNER JOIN l{lineNumber}_archive_component AS c ON bm.id_component = c.old_id 
        //          WHERE bm.batch_id IN 
        //           (SELECT id_batch  FROM l{lineNumber}_batchs as b WHERE b.id_application IN 
        //           ({Constants.DatabaseCommands.Applications.SelectApplicationsByFilter(database, lineNumber, filterData, values: "id_recipe")}))
				    //GROUP BY name;";
        public string SelectAllComponents(string database)
            => $@"SELECT * FROM {database}.`component`;";
        /*
         
    SELECT
      SUM(RealConsumption.mass) AS mass_real,
      SUM(RealConsumption.mass_auto) AS mass_real_auto,
      SUM(RealConsumption.mass_manual) AS mass_real_manual,
      SUM(RealConsumption.mass_correction) AS mass_real_correction,
      SUM(RealConsumption.mass_manual_correction) AS mass_manual_correction,
      SUM(RealConsumption.mass_humidity_correction) AS mass_humidity_correction,
      SUM(CASE WHEN lars.id_recipe > 0
    THEN lars.amount * la.volume_cur
    ELSE RealConsumption.mass
      END) AS mass_need,
      l1_archive_component.id_component AS component_id,
      l1_archive_component.old_id AS component_old_id,
      l1_archive_component.name AS component_name,
      type_comp.type AS component_type
    FROM l1_applications la
  
    INNER JOIN (SELECT
      lay.app_id,
      lay.recipe_id,
      SUM(lbm.mass_auto + lbm.mass_manual) AS mass,
      SUM(lbm.mass_auto) AS mass_auto,
      SUM(lbm.mass_manual) AS mass_manual,
      SUM(lbm.mass_correction) AS mass_correction,
      SUM(lbm.mass_manual_correction) AS mass_manual_correction,
      SUM(lbm.mass_humidity_correction) AS mass_humidity_correction,
      l1_archive_component.id_component AS archive_component_id,
      l1_archive_component.old_id AS archive_component_old_id

    FROM l1_batch_mats lbm
    INNER JOIN l1_batchs lb
    ON lbm.batch_id = lb.id_batch
    INNER JOIN l1_archive_component
    ON lbm.id_component = l1_archive_component.id_component
    INNER JOIN l1_layers lay
    ON lb.id_layer = lay.id
    WHERE lb.is_duplicate = 0
    GROUP BY lay.app_id,
    l1_archive_component.old_id) RealConsumption
    ON RealConsumption.app_id = la.id_application
    INNER JOIN l1_archive_recipe lar
    ON lar.id_recipe = RealConsumption.recipe_id 
    INNER JOIN l1_archive_component
    ON RealConsumption.archive_component_id = l1_archive_component.id_component
    INNER JOIN type_comp
    ON l1_archive_component.id_type = type_comp.id_type
    LEFT OUTER JOIN l1_archive_recipe_structure lars
    ON lars.id_recipe = lar.id_recipe
    AND lars.id_component = RealConsumption.archive_component_old_id 
    
    WHERE RealConsumption.mass <> 0 AND (mass <> 0 OR lars.amount <> 0) AND la.id_application IN (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16) 
    GROUP BY RealConsumption.archive_component_old_id 
    ORDER BY l1_archive_component.name;
         
         
         
         */
        public string SelectComponentExpenditures(string database, int lineNumber, FilterData filterData)
            => $@"SELECT
      SUM(RealConsumption.mass) AS mass_real,
      SUM(RealConsumption.mass_auto) AS mass_real_auto,
      SUM(RealConsumption.mass_manual) AS mass_real_manual,
      SUM(RealConsumption.mass_correction) AS mass_real_correction,
      SUM(RealConsumption.mass_manual_correction) AS mass_manual_correction,
      SUM(RealConsumption.mass_humidity_correction) AS mass_humidity_correction,
      SUM(CASE WHEN lars.id_recipe > 0
    THEN lars.amount * la.volume_cur
    ELSE RealConsumption.mass
      END) AS mass_need,
      l{lineNumber}_archive_component.id_component AS component_id,
      l{lineNumber}_archive_component.old_id AS component_old_id,
      l{lineNumber}_archive_component.name AS component_name,
      type_comp.type AS component_type
    FROM {database}.`l{lineNumber}_applications` la
  
    INNER JOIN (SELECT
      lay.app_id,
      lay.recipe_id,
      SUM(lbm.mass_auto + lbm.mass_manual) AS mass,
      SUM(lbm.mass_auto) AS mass_auto,
      SUM(lbm.mass_manual) AS mass_manual,
      SUM(lbm.mass_correction) AS mass_correction,
      SUM(lbm.mass_manual_correction) AS mass_manual_correction,
      SUM(lbm.mass_humidity_correction) AS mass_humidity_correction,
      l{lineNumber}_archive_component.id_component AS archive_component_id,
      l{lineNumber}_archive_component.old_id AS archive_component_old_id

    FROM {database}.`l{lineNumber}_batch_mats` lbm
    INNER JOIN l{lineNumber}_batchs lb
    ON lbm.batch_id = lb.id_batch
    INNER JOIN l{lineNumber}_archive_component
    ON lbm.id_component = l{lineNumber}_archive_component.id_component
    INNER JOIN l{lineNumber}_layers lay
    ON lb.id_layer = lay.id
    WHERE lb.is_duplicate = 0
    GROUP BY lay.app_id,
    l{lineNumber}_archive_component.old_id) RealConsumption
    ON RealConsumption.app_id = la.id_application
    INNER JOIN l{lineNumber}_archive_recipe lar
    ON lar.id_recipe = RealConsumption.recipe_id 
    INNER JOIN l{lineNumber}_archive_component
    ON RealConsumption.archive_component_id = l{lineNumber}_archive_component.id_component
    INNER JOIN type_comp
    ON l{lineNumber}_archive_component.id_type = type_comp.id_type
    LEFT OUTER JOIN l{lineNumber}_archive_recipe_structure lars
    ON lars.id_recipe = lar.id_recipe
    AND lars.id_component = RealConsumption.archive_component_old_id 
    
    WHERE RealConsumption.mass <> 0 AND (mass <> 0 OR lars.amount <> 0) AND la.id_application IN ({Constants.DatabaseCommands.Applications.SelectApplicationsByFilter(database, lineNumber, filterData, values: "id_application")}) 
    GROUP BY RealConsumption.archive_component_old_id 
    ORDER BY l{lineNumber}_archive_component.name;";
    }
}