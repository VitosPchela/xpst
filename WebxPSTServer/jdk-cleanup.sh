for f in jre/lib/charsets.jar jre/lib/ext/sunjce_provider.jar jre/lib/ext/localedata.jar jre/lib/ext/ldapsec.jar jre/lib/ext/dnsns.jar db demo sample src.zip
do
	rm -rf jdk1.6/$f
done

for f in webapps/docs webapps/examples webapps/host-manager webapps/manager
do
	rm -rf apache-tomcat-6.0.18/$f
done

