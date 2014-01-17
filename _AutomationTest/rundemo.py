#! /usr/bin/env python


Examples:
  rundemo.py test                        # Run all tests in a directory
  rundemo.py --variable BROWSER:IE test  # Override variable
  rundemo.py -v BROWSER:IE -v DELAY:0.5 test
  rundemo.py --variable SUT:flex test

import os
import sys
from tempfile import TemporaryFile
from subprocess import Popen, call, STDOUT

try:
    import Selenium2Library
except ImportError:
    print 'Importing SeleniumLibrary module failed.'
    print 'Please make sure you have SeleniumLibrary installed.'
    sys.exit(1)


ROOT = os.path.dirname(os.path.abspath(__file__))
DEMOAPP = os.path.join(ROOT, 'demoapp', 'server.py')


def run_tests(args):
    start_selenium_server()
    start_demo_application()
    call(['pybot'] + args, shell=(os.sep == '\\'))
    stop_demo_application()
    stop_selenium_server()

def start_demo_application():
    Popen(['python', DEMOAPP, 'start'], stdout=TemporaryFile(), stderr=STDOUT)

def stop_demo_application():
    call(['python', DEMOAPP, 'stop'], stdout=TemporaryFile(), stderr=STDOUT)

def start_selenium_server():
    logfile = open(os.path.join(ROOT, 'selenium_log.txt'), 'w')
    SeleniumLibrary.start_selenium_server(logfile)

def stop_selenium_server():
    SeleniumLibrary.shut_down_selenium_server()

def print_help():
    print __doc__

def print_usage():
    print 'Usage: rundemo.py [options] datasource'
    print '   or: rundemo.py demoapp start|stop'
    print '   or: rundemo.py selenium start|stop'
    print '   or: rundemo.py help'


if __name__ == '__main__':
    action = {'demoapp-start': start_demo_application,
              'demoapp-stop': stop_demo_application,
              'selenium-start': start_selenium_server,
              'selenium-stop': stop_selenium_server,
              'help': print_help,
              '': print_usage}.get('-'.join(sys.argv[1:]))
    if action:
        action()
    else:
        run_tests(sys.argv[1:])
