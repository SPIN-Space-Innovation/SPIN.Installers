// MIT License
//
// Copyright (c) 2024 SPIN - Space Innovation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Reflection;

namespace SPIN.Installers;

[Priority(uint.MaxValue / 2)]
public class MediatorInstaller : IInstaller
{
    public Task InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddMediatR(cfg =>
        {
            List<IMediatorInstaller> mediatorInstallers = [.. AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                {
                    bool isOfTypeIMediatorInstaller = typeof(IMediatorInstaller).IsAssignableFrom(type);
                    bool isNotInterface = !type.IsInterface;
                    bool isNotAbstract = !type.IsAbstract;

                    return isOfTypeIMediatorInstaller && isNotInterface && isNotAbstract;
                })
                .Select(Activator.CreateInstance)
                .Cast<IMediatorInstaller>()
                .OrderBy(installer =>
                {
                    PriorityAttribute? priorityAttribute = installer.GetType().GetCustomAttribute<PriorityAttribute>();

                    return priorityAttribute?.PriorityLevel ?? uint.MaxValue;
                })];

            mediatorInstallers.ForEach(installer =>
            {
                installer.InstallService(cfg, serviceCollection, configuration);
            });
        });

        return Task.CompletedTask;
    }
}
