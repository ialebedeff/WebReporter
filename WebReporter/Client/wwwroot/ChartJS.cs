﻿using Microsoft.JSInterop;
using ChartJs.Blazor.Common;

namespace WebReporter.Client.Rpc
{
    public class ChartJS : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public ChartJS(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./js/chart.js").AsTask());
        }

        /// <summary>
        /// Обновление диаграмм
        /// </summary>
        /// <returns></returns>
        public async ValueTask UpdateCharts()
        {
            var module = await moduleTask.Value;

            // Вызов удалённой процедуры (updateCharts) из файла chart.js
            await module.InvokeVoidAsync("updateCharts");
        }

        /// <summary>
        /// Диаграмма производительности на замес
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="batchNumbers"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public async ValueTask PerformanceOnBatchChart(List<double> dataset, List<string> batchNumbers, string label)
        {
            var module = await moduleTask.Value;

            // Вызов удалённой процедуры (performanceOnBatch) из файла chart.js
            await module.InvokeVoidAsync("performanceOnBatch", dataset, batchNumbers, label);
        }

        /// <summary>
        /// Диаграмма средней фактической / потенциальной
        /// </summary>
        /// <param name="dataset1"></param>
        /// <param name="dataset2"></param>
        /// <param name="dates"></param>
        /// <param name="labels"></param>
        /// <returns></returns>
        public async ValueTask FactPotentialBarChart(List<double> dataset1, List<double> dataset2, List<string> dates, List<string> labels)
        {
            var module = await moduleTask.Value;

            // Вызов удалённой процедуры (factPotentialBarChart) из файла chart.js
            await module.InvokeVoidAsync("factPotentialBarChart", dataset1, dataset2, dates, labels);
        }

        /// <summary>
        /// Диграмма заказанного/выполненного объёма 
        /// </summary>
        /// <param name="dataset1"></param>
        /// <param name="dataset2"></param>
        /// <param name="dates"></param>
        /// <param name="labels"></param>
        /// <returns></returns>
        public async ValueTask OrderedVolumeChart(
            IEnumerable<float> dataset1, 
            IEnumerable<float> dataset2, 
            IEnumerable<string> dates,
            IEnumerable<string> labels)
        {
            var module = await moduleTask.Value;

            // Вызов удалённой процедуры (orderedVolume) из файла chart.js
            await module.InvokeVoidAsync("orderedVolume", dataset1, dataset2, dates, labels);
        }

        /// <summary>
        /// Диаграмма Гантта
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public async ValueTask GanttChart(List<ChartData> data, List<string> categories)
        {
            var module = await moduleTask.Value;

            // Вызов удалённой процедуры (jsGantt) из файла chart.js
            await module.InvokeVoidAsync("highchartXRangeGantt", data, categories);
        }

        public async ValueTask GanttChart(List<ChartData> data, List<string> categories, object obj)
        {
            var module = await moduleTask.Value;

            // Вызов удалённой процедуры (jsGantt) из файла chart.js
            await module.InvokeVoidAsync("highchartXRangeGantt", data, categories);
        }

        /// <summary>
        /// Диаграмма расходов по бункерам
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataset"></param>
        /// <param name="labels"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public async ValueTask BunkerExpendituresChart<T>(Dictionary<int, List<T>> dataset, List<string> labels, List<int> categories)
        {
            var module = await moduleTask.Value;

            // Вызов удалённой процедуры (expendituresBarChart) из файла chart.js
            await module.InvokeVoidAsync("expendituresBarChart", dataset, labels, categories);
        }

        /// <summary>
        /// Диаграмма остатков по бункерам
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataset"></param>
        /// <param name="labels"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public async ValueTask BunkerRemainsChart<T>(Dictionary<int, List<T>> dataset, List<string> labels, List<int> categories)
        {
            var module = await moduleTask.Value;

            // Вызов удалённой процедуры (remainsBarChart) из файла chart.js
            await module.InvokeVoidAsync("remainsBarChart", dataset, labels, categories);
        }

        /// <summary>
        /// Очищение памяти JS объекта
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
