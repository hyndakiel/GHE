﻿using GitHubExtractor.Dtos;
using GitHubExtractor.Services;
using GitHubExtractor.Services.Connection;
using NLog;
using System;
using System.Text;

namespace GitHubExtractor
{
	class Program
	{
		public static readonly Logger LOG = LogManager.GetCurrentClassLogger();

		protected Program() { }

		static void Main(string[] args)
		{
			InitializeApp();
			LOG.Info("INIT");
			try
			{
				string gitRequestUser = args[0];
				string gitRequestToken = args[1];

				BasicAuth basicAuth = new BasicAuth(gitRequestUser, gitRequestToken);

				GitHubRequestDto gitHubRequestDto = new GitHubRequestDto(basicAuth);

				string connectionKey = "basePathGitHubApi";
				GitHubApiConnectionService gitHubApiConnectionService = new GitHubApiConnectionService(connectionKey);

				GitHubPullRequestService gitHubPullRequestService = new GitHubPullRequestService(gitHubRequestDto, gitHubApiConnectionService);
				GitHubIssuesRequestService gitHubIssuesRequestService = new GitHubIssuesRequestService(gitHubRequestDto, gitHubApiConnectionService);
				GitHubCommitRequestService gitHubCommitRequestService = new GitHubCommitRequestService(gitHubRequestDto, gitHubApiConnectionService);

				GitHubService gitHubService = new GitHubService(gitHubPullRequestService, gitHubIssuesRequestService, gitHubCommitRequestService);

				gitHubService.CreatePullRequestCSVFile();
			}
			catch (Exception e)
			{
				LOG.Error(String.Format("Unexpected error: {0}", e));
			}
			LOG.Info("END");
		}

		private static void InitializeApp()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}
	}
}
