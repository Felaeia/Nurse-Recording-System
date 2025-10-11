import 'package:flutter/material.dart';
import 'package:logging/logging.dart';
import 'package:device_preview/device_preview.dart';
import 'routes/app_routes.dart';

void main() {
  _setupLogging();
  runApp(
    DevicePreview(
      enabled: true,
      builder: (context) => const ClinicApp(),
    ),
  );
}

void _setupLogging() {
  Logger.root.level = Level.ALL;
  Logger.root.onRecord.listen((record) {
    // ignore: avoid_print
    print(
        '${record.level.name}: ${record.time}: ${record.loggerName}: ${record.message}');
  });
}

class ClinicApp extends StatelessWidget {
  const ClinicApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      theme: ThemeData(fontFamily: 'Poppins'),
      builder: DevicePreview.appBuilder,
      initialRoute: AppRoutes.home,
      routes: AppRoutes.getRoutes(), 
    );
  }
}
