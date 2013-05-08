<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="dot_dep_customerinsmember_ag">
        <table class="tblDesc">
            <tr>
              <xsl:choose>
                <xsl:when test="@fname">
                  <td class="colW1">{lbl:FirstName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@fname" />
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1">{lbl:FirstName}</td>
                  <td class="colW2">

                    <xsl:value-of select="@dot_dep_customerinsmember_agfirstname" />
                  </td>

                </xsl:otherwise>
              </xsl:choose>
              <xsl:choose>
                <xsl:when test="@lname">
                  <td class="colW1">{lbl:LastName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@lname" />
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1">{lbl:LastName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@lastname" />
                  </td>

                </xsl:otherwise>
              </xsl:choose>
              
                <td class="colW1">{lbl:Email}</td>
                <td class="colW2">
                    <xsl:value-of select="@email" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:DepositUserName}</td>
                <td class="colW2">
                    <xsl:value-of select="@depositusername" />
                </td>
                <xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:DepositUserID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@deposituserid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>
              <td class="colW1">{lbl:OddsType}</td>
              <td class="colW2">
                <xsl:value-of select="@oddstype" />
              </td>

            </tr>
            <tr>
              <td class="colW1">{lbl:Username}</td>
              <td class="colW2">
                <xsl:value-of select="@username" />
              </td>
              <xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:AgentID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@agentid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>
                <xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:AffiliateID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@affiliateid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>
            </tr>
          <tr>
            <td class="colW1">{lbl:Creator}</td>
            <td class="colW2">
              <xsl:value-of select="@creator" />
            </td>
            <td class="colW1">{lbl:BonusCode}</td>
            <td class="colW2">
              <xsl:value-of select="@bonuscode" />
            </td>
            <td class="colW1">{lbl:Recommend}</td>
            <td class="colW2">
              <xsl:value-of select="@recommend" />
            </td>
          </tr>
          
          <tr>
            <td class="colW1">{lbl:Ugroup}</td>
            <td class="colW2">
              <xsl:value-of select="@ugroup" />
            </td>
            <td class="colW1">{lbl:Success}</td>
            <td class="colW2">
              <xsl:value-of select="@success" />
            </td>
            <td class="colW1">{lbl:ErrCode}</td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
          </tr>
          
          
            <tr>
                <td class="colW1">{lbl:ErrMsg}</td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
              <xsl:choose>
                <xsl:when test="@agentpt">
                  <td class="colW1">{lbl:AgentPT}</td>
                  <td class="colW2">
                    <xsl:value-of select="@agentpt" />
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1"></td>
                  <td class="colW2">
                  </td>
                </xsl:otherwise>
              </xsl:choose>
              <xsl:choose>
                <xsl:when test="@discount">
                  <td class="colW1">{lbl:Discount}</td>
                  <td class="colW2">
                    <xsl:value-of select="@discount" />
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1"></td>
                  <td class="colW2">
                  </td>
                </xsl:otherwise>
              </xsl:choose>
            </tr>
          <xsl:choose>
            <xsl:when test="@suspended">
              <tr>
                <td class="colW1">{lbl:Suspended}</td>
                <td class="colW2">
                  <xsl:value-of select="@suspended" />
                </td>
                <td class="colW1">{lbl:DisableHorseRacing}</td>
                <td class="colW2">
                  <xsl:value-of select="@disablehorseracing" />
                </td>
                <td class="colW1">{lbl:DisableFinancial}</td>
                <td class="colW2">
                  <xsl:value-of select="@disablefinancial" />
                </td>
              </tr>
            </xsl:when>
          </xsl:choose>
          <xsl:choose>
            <xsl:when test="@minbet">
              <tr>
                <td class="colW1">{lbl:MinBet}</td>
                <td class="colW2">
                  <xsl:value-of select="@minbet" />
                </td>
                <td class="colW1">{lbl:MaxBet}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxbet" />
                </td>
                <td class="colW1">{lbl:MaxPerMatch}</td>
                <td class="colW2">
                  <xsl:value-of select="@maxpermatch" />
                </td>
              </tr>
            </xsl:when>
          </xsl:choose>
          
          
        </table>

    </xsl:template>
</xsl:stylesheet>
